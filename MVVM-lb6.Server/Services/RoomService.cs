using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
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

    public ServiceResult<List<Room>> GetAll()
    {
	    return new ServiceResult<List<Room>>(Context.Rooms.ToList())
	    {
		    Message = new string[] {SuccessMessages.Room.Extracted},
		    IsSuccessful = true
	    };
    }
    
    public ServiceResult<List<Room>> GetAvailable()
    {
	    return new ServiceResult<List<Room>>(Context.Rooms.Where(r => r.IsAvailable.Equals(true)).ToList())
	    {
		    Message = new string[] {SuccessMessages.Room.Extracted},
		    IsSuccessful = true
	    };
    }

    public ServiceResult Create(CreateRoomRequest request)
    {
	    var room = Context.Rooms.FirstOrDefault(r => r.RealNumber.Equals(request.RealNumber));
		if (room is not null)
			return new ServiceResult(ErrorMessages.Room.ThereIsNumber) {IsSuccessful = false};
		
		
		var newRoom = new Room
		{
			RoomId = Guid.NewGuid(),
			BedsNumber = request.BedsNumber,
			IsAvailable = request.IsAvailable,
			PricePerDay = request.PricePerDay,
			RealNumber = request.RealNumber
		};

		Context.Rooms.Add(newRoom);
		Context.SaveChanges();

		return new ServiceResult(SuccessMessages.Room.Created) {IsSuccessful = true};
	}
    
	public ServiceResult Delete(Guid id)
	{
		var room = Context.Rooms.FirstOrDefault(r => 
			r.RoomId.Equals(id));

		if (room is null) 
			return new ServiceResult(new [] {ErrorMessages.Room.NotFound}) {IsSuccessful = false};
		
		Context.Rooms.Remove(room);
		Context.SaveChanges();

		return new ServiceResult(new[] { SuccessMessages.Room.Deleted }) { IsSuccessful = true };
	}

	public ServiceResult Update(UpdateRoomRequest request)
	{
		var room = Context.Rooms.FirstOrDefault(r => r.RoomId.Equals(request.RoomId));
		if (room is null)
			return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };
		
		var realNumberOfRoom = Context.Rooms.FirstOrDefault(r => r.RealNumber.Equals(request.RealNumber));
		if (realNumberOfRoom is not null)
			return new ServiceResult(ErrorMessages.Room.ThereIsNumber) {IsSuccessful = false};


		room.BedsNumber = request.BedsNumber;
		room.RealNumber = request.RealNumber;
		room.IsAvailable = request.IsAvailable;
		room.PricePerDay = request.PricePerDay;
		
		Context.SaveChanges();

		return new ServiceResult(new[] { SuccessMessages.Room.Updated }) { IsSuccessful = true };
	}
	
	public ServiceResult BookRoom(BookingStayRoomRequest request)
	{
		var room = Context.Rooms.FirstOrDefault(r => r.RoomId.Equals(request.RoomId));
		if (room is null)
			return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

		Dictionary<DateTime, DateTime> dataDictionary = GetDatesDictionary(request);
		
		foreach (var date in dataDictionary)
		{
			if (!((date.Key < request.FirstDate && date.Value < request.FirstDate) ||
			      (date.Key > request.SecondDate && date.Value > request.SecondDate)))
			{
				return new ServiceResult(new[] { ErrorMessages.Room.DatesOverlap }) { IsSuccessful = false };
			}
		}

		room.BookedDates.Add(request.FirstDate, request.SecondDate);

		Context.SaveChanges();

		return new ServiceResult(new[] { SuccessMessages.Room.Updated }) { IsSuccessful = true };
	}
	
	public ServiceResult StayInRoom (BookingStayRoomRequest request)
	{
		var room = Context.Rooms.FirstOrDefault(r => r.RoomId.Equals(request.RoomId));
		if (room is null)
			return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

		Dictionary<DateTime, DateTime> dataDictionary = GetDatesDictionary(request);
		
		foreach (var date in dataDictionary)
		{
			if (!((date.Key < request.FirstDate && date.Value < request.FirstDate) ||
			      (date.Key > request.SecondDate && date.Value > request.SecondDate)))
			{
				return new ServiceResult(new[] { ErrorMessages.Room.DatesOverlap }) { IsSuccessful = false };
			}
		}

		room.DateOfStay.Add(request.FirstDate, request.SecondDate);

		Context.SaveChanges();

		return new ServiceResult(new[] { SuccessMessages.Room.Updated }) { IsSuccessful = true };
	}

	private Dictionary<DateTime, DateTime> GetDatesDictionary(BookingStayRoomRequest request)
	{
		Dictionary<DateTime, DateTime> dataDictionary = new Dictionary<DateTime, DateTime>();
		
		Context.Rooms
			.SelectMany(r => r.BookedDates)
			.ToList()
			.ForEach(d => dataDictionary.Add(d.Key, d.Value));
		
		Context.Rooms
			.SelectMany(r => r.DateOfStay)
			.ToList()
			.ForEach(d => dataDictionary.Add(d.Key, d.Value));

		return dataDictionary;
	}
}