using System;
using System.ComponentModel.DataAnnotations;

namespace MVVM_lb6.Domain.Requests
{
    [Serializable]
    public class BookingStayRoomRequest
    {
        [Required]
        public Guid RoomId { get; set; }

        [Required, DataType(DataType.Date)]
        public DateTime FirstDate { get; set; }
        [Required, DataType(DataType.Date)]
        public DateTime SecondDate { get; set; }
    }
}
