using System;
using System.ComponentModel.DataAnnotations;

namespace MVVM_lb6.Domain.Requests
{
    [Serializable]
    public class CreateRoomRequest
    {
        [Required]
        public ushort RealNumber { get; set; }

        [Required] 
        public byte BedsNumber { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }
    }
}
