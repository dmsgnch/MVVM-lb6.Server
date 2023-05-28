using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.IdentityModel.Tokens;
using MVVM_lb6.Domain.Models;
using MVVM_lb6.Domain.Requests;
using MVVM_lb6.Domain.Responses;
using MVVM_lb6.Server.Helpers;
using MVVM_lb6.Server.Models;
using Newtonsoft.Json;
using Server.Common.Constants;
using Server.Domain;
using Server.Services.Abstract;

namespace Server.Services;

public class RoomService : IRoomService
{
    public Settings Settings { get; init; }
    public ApplicationDbContext Context { get; init; }

    public RoomService(Settings settings, ApplicationDbContext context)
    {
        Settings = settings;
        Context = context;
    }

    /// <summary>
    /// Pull out the full list of rooms
    /// </summary>
    /// <returns>Room list</returns>
    public ServiceResult<List<Room>> GetAll()
    {
        return new ServiceResult<List<Room>>(Context.Rooms.ToList())
        {
            Message = new string[] { SuccessMessages.Room.Extracted },
            IsSuccessful = true
        };
    }

    /// <summary>
    /// Pull out list of rooms where rooms are available
    /// </summary>
    /// <returns>Room list</returns>
    public ServiceResult<List<Room>> GetAvailable()
    {
        return new ServiceResult<List<Room>>(Context.Rooms.Where(r => r.IsAvailable.Equals(true)).ToList())
        {
            Message = new string[] { SuccessMessages.Room.Extracted },
            IsSuccessful = true
        };
    }

    /// <summary>
    /// Create a new room and adds it to the database according to the received data
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Object contains info about operation</returns>
    public ServiceResult Create(CreateRoomRequest request)
    {
        if (IsRoomByRealNumberExist(request.RealNumber, out Room? room))
            return new ServiceResult(ErrorMessages.Room.ThereIsNumber) { IsSuccessful = false };

        var newRoom = new Room
        {
            RoomId = Guid.NewGuid(),

            BedsNumber = request.BedsNumber,
            IsAvailable = request.IsAvailable,
            PricePerDay = request.PricePerDay,
            RealNumber = request.RealNumber,

            BookedDates = new(),
            DateOfStay = new()
        };

        Context.Rooms.Add(newRoom);
        Context.SaveChanges();

        CheckStateStatus();

        return new ServiceResult(SuccessMessages.Room.Created) { IsSuccessful = true };
    }

    /// <summary>
    /// Removes a room which has own id is equal to received
    /// </summary>
    /// <param name="id"></param>
    /// <returns>Object contains info about operation</returns>
    public ServiceResult Delete(Guid id)
    {
        if (!IsRoomByIdExist(id, out Room? room))
            return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

        Context.Rooms.Remove(room);
        Context.SaveChanges();

        CheckStateStatus();

        return new ServiceResult(new[] { SuccessMessages.Room.Deleted }) { IsSuccessful = true };
    }

    /// <summary>
    /// Update a room which has own id is equal to received
    /// </summary>
    /// <param name="request"></param>
    /// <returns>Object contains info about operation</returns>
    public ServiceResult Update(UpdateRoomRequest request)
    {
        if (!IsRoomByIdExist(request.RoomId, out Room? room))
            return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

        if (!room.RealNumber.Equals(request.RealNumber) &&
            IsRoomByRealNumberExist(request.RealNumber, out Room? room2))
        {
            return new ServiceResult(ErrorMessages.Room.ThereIsNumber) { IsSuccessful = false };
        }

        room.BedsNumber = request.BedsNumber;
        room.RealNumber = request.RealNumber;
        room.IsAvailable = request.IsAvailable;
        room.PricePerDay = request.PricePerDay;

        Context.SaveChanges();

        CheckStateStatus();

        return new ServiceResult(new[] { SuccessMessages.Room.Updated }) { IsSuccessful = true };
    }

    public ServiceResult BookRoom(BookingStayRoomRequest request)
    {
        if (!IsRoomByIdExist(request.RoomId, out Room? room))
            return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

        if (!IsTimeRangesNotOverlap(request))
            return new ServiceResult(new[] { ErrorMessages.Room.DatesOverlap }) { IsSuccessful = false };

        Context.BookTimeRanges.Add(new BookTimeRange()
        {
            StartTime = request.FirstDate,
            FinishTime = request.SecondDate,

            RoomId = request.RoomId,
            BookedRoom = room
        });

        Context.SaveChanges();

        CheckStateStatus();

        return new ServiceResult(new[] { SuccessMessages.Room.SuccessfulBooking }) { IsSuccessful = true };
    }

    public ServiceResult StayInRoom(BookingStayRoomRequest request)
    {
        if (!IsRoomByIdExist(request.RoomId, out Room? room))
            return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

        if (!IsTimeRangesNotOverlap(request))
            return new ServiceResult(new[] { ErrorMessages.Room.DatesOverlap }) { IsSuccessful = false };

        Context.SettledTimeRanges.Add(new SettledTimeRange()
        {
            StartTime = request.FirstDate,
            FinishTime = request.SecondDate,

            RoomId = request.RoomId,
            SettledRoom = room
        });
        room.IsAvailable = false;

        Context.SaveChanges();

        CheckStateStatus();

        return new ServiceResult(new[] { SuccessMessages.Room.SuccessfulSettling }) { IsSuccessful = true };
    }

    private List<TimeRangeBase> GetDatesRangesFromDb(Guid roomId)
    {
        List<TimeRangeBase> bookedDates = Context.BookTimeRanges
            .ToList()
            .Where(tr => tr.RoomId.Equals(roomId))
            .Select(tr => tr as TimeRangeBase)
            .ToList();

        List<TimeRangeBase> settlingDates = Context.SettledTimeRanges
            .ToList()
            .Where(tr => tr.RoomId.Equals(roomId))
            .Select(tr => tr as TimeRangeBase)
            .ToList();

        bookedDates.AddRange(settlingDates);
        return bookedDates;
    }

    private void CheckStateStatus()
    {
        List<Room> allRooms = GetAll()?.Value ?? new List<Room>();
        List<Room> availableRooms = GetAvailable()?.Value ?? new List<Room>();

        if (StateMachine.HotelState is HotelState.HolidaySeason)
        {
            if (availableRooms.Count.Equals(0))
            {
                StateMachine.ChangeState(HotelState.GuestAccommodationStop);
            }
        }
        else if (StateMachine.HotelState is HotelState.GuestAccommodationStop)
        {
            if (Math.Ceiling((double)availableRooms.Count / (double)allRooms.Count * 100) < 90 &&
                allRooms.Count >= 5)
            {
                StateMachine.ChangeState(HotelState.HolidaySeason);
            }
        }
    }

    /// <summary>Check that room with received id exist</summary>
    /// <returns>Return true value and room object if such room exist</returns>
    private bool IsRoomByIdExist(Guid roomId, out Room? room)
    {
        room = Context.Rooms.FirstOrDefault(r => r.RoomId.Equals(roomId));
        return room is not null;
    }

    /// <summary>Check that room with received real number exist</summary> 
    /// <returns>Return true value and room object if such room exist</returns>
    private bool IsRoomByRealNumberExist(ushort realId, out Room? room)
    {
        room = Context.Rooms.FirstOrDefault(r => r.RealNumber.Equals(realId));
        return room is not null;
    }

    private bool IsTimeRangesNotOverlap(BookingStayRoomRequest request)
    {
        List<TimeRangeBase> dataDictionary = GetDatesRangesFromDb(request.RoomId);

        foreach (var date in dataDictionary)
        {
            if (!(date.FinishTime < request.FirstDate ||
                  date.StartTime > request.SecondDate))
            {
                return false;
            }
        }

        return true;
    }
}