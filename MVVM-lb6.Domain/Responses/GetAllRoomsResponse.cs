using System.Collections.Generic;
using MVVM_lb6.Domain.Models;
using MVVM_lb6.Domain.Responses.Abstract;

namespace MVVM_lb6.Domain.Responses
{
    public class GetAllRoomsResponse : ResponseBase
    {
        public IList<Room> Rooms { get; set; }
    }
}