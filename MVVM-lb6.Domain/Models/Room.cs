using System;

namespace MVVM_lb6.Domain.Models
{
    [Serializable]
    public class Room
    {
        public Guid RoomId { get; set; }

        public int BedsNumber { get; set; }
        public bool IsAvailable { get; set; }

        public Guid HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}