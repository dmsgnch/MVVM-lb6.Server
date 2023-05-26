using MVVM_lb6.Domain.Models;
using MVVM_lb6.Domain.Requests;
using MVVM_lb6.Domain.Responses;
using Server.Domain;

namespace Server.Services.Abstract;

public interface IRoomService
{
    ServiceResult<List<Room>> GetAll();
    ServiceResult<List<Room>> GetAvailable();
    ServiceResult Create(CreateRoomRequest request);
    ServiceResult Delete(Guid id);
    ServiceResult Update(UpdateRoomRequest request);
}
