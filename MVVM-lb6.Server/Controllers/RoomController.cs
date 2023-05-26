using System.Data;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MVVM_lb6.Domain.Models;
using MVVM_lb6.Domain.Requests;
using MVVM_lb6.Domain.Responses;
using MVVM_lb6.Domain.Routes;
using Server.Common.Constants;
using Server.Domain;
using Server.Services.Abstract;

namespace Server.Controllers;

[Authorize]
[ApiController]
[Route("[controller]")]
public class RoomController : ControllerBase
{
    private readonly IRoomService _roomService;
    
    public RoomController(IRoomService roomService)
    {
        _roomService = roomService;
    }
    
    [HttpGet, Route(ApiRoutes.Room.GetAllRooms)]
    public async Task<IActionResult> GetAllRooms()
    {
        ServiceResult<List<Room>> result = _roomService.GetAll();

        if (result.IsSuccessful is false)
            throw new DataException();
        
        return Ok(new GetAllRoomsResponse()
        {
            Info = result.Message,
            Rooms = result?.Value ?? throw new DataException()
        });
    }
    
    [HttpPost, Route(ApiRoutes.Room.Create)]
    public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest request)
    {
        var result = _roomService.Create(request);

        if (result.IsSuccessful is false)
            return BadRequest(new LambdaResponse() { Info = result.Message });
        
        return Ok(new LambdaResponse() { Info = result.Message });
    }

    [HttpDelete, Route(ApiRoutes.Room.Delete)]
    public async Task<IActionResult> DeleteLobby([FromRoute] Guid id)
    {
        var result = _roomService.Delete(id);

        if (result.IsSuccessful is false)
            return BadRequest(new LambdaResponse() { Info = result.Message });
        
        return Ok(new LambdaResponse() { Info = result.Message });
    }
    
    [HttpPut, Route(ApiRoutes.Room.Update)]
    public async Task<IActionResult> UpdateLobby([FromBody] UpdateRoomRequest request)
    {
        var result = _roomService.Update(request);

        if (result.IsSuccessful is false)
            return BadRequest(new LambdaResponse() { Info = result.Message });
        
        return Ok(new LambdaResponse() { Info = result.Message });
    }
}