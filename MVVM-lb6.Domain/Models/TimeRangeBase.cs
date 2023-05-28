using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MVVM_lb6.Domain.Models
{
    public class TimeRangeBase
    {
        [DataType(DataType.Date)]
        public DateTime StartTime { get; set; }
        [DataType(DataType.Date)]
        public DateTime FinishTime { get; set; }
    }
}