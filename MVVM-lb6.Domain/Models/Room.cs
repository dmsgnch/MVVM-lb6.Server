using System;
using System.Data;

namespace MVVM_lb6.Domain.Models
{
    [Serializable]
    public class Room
    {
        public Guid RoomId { get; set; }
        
        private ushort _realNumber;
        private byte _bedsNumber;
        private decimal _pricePerDay;

        public ushort RealNumber
        {
            get => _realNumber;
            set
            {
                if (value < 0) throw new DataException("Unreal data value");

                _realNumber = value;
            }
        }

        public byte BedsNumber
        {
            get => _bedsNumber;
            set
            {
                if (value < 0 || value > 256) throw new DataException("Unreal data value");

                _bedsNumber = value;
            }
        }

        public decimal PricePerDay
        {
            get => _pricePerDay;
            set
            {
                if (value < 0) throw new DataException("Price can`t have negative value");

                _pricePerDay = value;
            }
        }

        public bool IsAvailable { get; set; }

        public Guid HotelId { get; set; }
        public Hotel? Hotel { get; set; }
    }
}