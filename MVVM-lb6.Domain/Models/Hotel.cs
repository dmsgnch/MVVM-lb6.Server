using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Linq;

namespace MVVM_lb6.Domain.Models
{
    [Table("Hotels"), Serializable]
    public class Hotel
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
        private Guid HotelId { get; set; }
        
        [Range(typeof(decimal), "0", "1000000", ErrorMessage = "Value must be greater than or equal to 0.")]
        public decimal Cash { get; set; } = 0;
    }
}