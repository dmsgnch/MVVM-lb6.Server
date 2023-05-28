using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM_lb6.Domain.Models
{
    [Table("SettledTimeRanges"), Serializable]
    public class SettledTimeRange : TimeRangeBase
    {
        [Key]
        public Guid SettledTimeRangeId { get; set; }
        
        [ForeignKey(nameof(Room))]
        public Guid RoomId { get; set; }
        public Room? SettledRoom { get; set; }
    }
}