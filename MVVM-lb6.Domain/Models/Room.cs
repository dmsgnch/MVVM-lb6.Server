using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Diagnostics.CodeAnalysis;

namespace MVVM_lb6.Domain.Models
{
    [Table("Rooms"), Serializable]
    public class Room
    {
        [Key]
        public Guid RoomId { get; set; }
    
        [Range(0, ushort.MaxValue)] 
        public ushort RealNumber { get; set; }

        [Range(0, byte.MaxValue)] 
        public byte BedsNumber { get; set; }

        [Range(typeof(decimal), "0", "9999", ErrorMessage = "Value must be greater than or equal to 0.")]
        public decimal PricePerDay { get; set; }

        public bool IsAvailable { get; set; } = true;

        [NotMapped]
        public List<BookTimeRange> BookedDates { get; set; } = new List<BookTimeRange>();
        [NotMapped]
        public List<BookTimeRange> DateOfStay { get; set; } = new List<BookTimeRange>();
    }
}