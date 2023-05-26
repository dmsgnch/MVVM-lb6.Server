using System;
using System.ComponentModel.DataAnnotations;

namespace MVVM_lb6.Domain.Requests
{
    [Serializable]
    public class UpdateRoomRequest
    {
        [Required] 
        public Guid RoomId { get; set; }
        
        [Required]
        public ushort RealNumber { get; set; }

        [Required] 
        public byte BedsNumber { get; set; }

        [Required, DataType(DataType.Currency)]
        public decimal PricePerDay { get; set; }
        
        [Required] 
        public bool IsAvailable { get; set; }
    }
}
