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

    public ServiceResult Create(CreateRoomRequest request)
	{
		var room = new Room
		{
			RoomId = Guid.NewGuid(),
			BedsNumber = request.BedsNumber,
			IsAvailable = true,
			PricePerDay = request.PricePerDay,
			RealNumber = request.RealNumber
		};

		Context.Rooms.Add(room);
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
		var room = Context.Rooms.FirstOrDefault(r =>
			r.RoomId.Equals(request.RoomId));

		if (room is null)
			return new ServiceResult(new[] { ErrorMessages.Room.NotFound }) { IsSuccessful = false };

		Context.Rooms.Update(new Room()
		{
			RoomId = request.RoomId,
			BedsNumber = request.BedsNumber,
			IsAvailable = request.IsAvailable,
			PricePerDay = request.PricePerDay,
			RealNumber = request.RealNumber
		});
		Context.SaveChanges();

		return new ServiceResult(new[] { SuccessMessages.Room.Updated }) { IsSuccessful = true };
	}
}