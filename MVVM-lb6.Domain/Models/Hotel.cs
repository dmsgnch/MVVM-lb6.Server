using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace MVVM_lb6.Domain.Models
{
    public class Hotel
    {
        private Guid HotelId { get; set; }
        
        private decimal _cash = 0;

        private IEnumerable<Room> Rooms { get; set; } =
            new ObservableCollection<Room>();

        public decimal Cash
        {
            get => _cash;
            set
            {
                if (value < 0) throw new DataException("Cash can`t have negative value");

                _cash = value;
            }
        }

        public int RoomsNumber { get => Rooms.Count(); }
    }
}