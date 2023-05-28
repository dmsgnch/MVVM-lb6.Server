using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM_lb6.Domain.Models
{
    [Table("BookTimeRanges"), Serializable]
    public class BookTimeRange : TimeRangeBase
    {
        [Key]
        public Guid BookTimeRangeId { get; set; }
        
        [ForeignKey(nameof(Room))]
        public Guid RoomId { get; set; }
        public Room? BookedRoom { get; set; }
    }
}