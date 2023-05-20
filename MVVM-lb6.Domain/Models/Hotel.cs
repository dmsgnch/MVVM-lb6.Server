using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MVVM_lb6.Domain.Models
{
    public class Hotel
    {
        public Guid HotelId { get; set; }

        public IList<Room> Rooms { get; set; } = new List<Room>();
        public int RoomsNumber { get => Rooms.Count(); }
    }
}