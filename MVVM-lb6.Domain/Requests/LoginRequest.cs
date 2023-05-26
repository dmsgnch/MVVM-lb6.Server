using System;
using System.ComponentModel.DataAnnotations;

namespace MVVM_lb6.Domain.Requests
{
    [Serializable]
    public class LoginRequest
    {
        [Required]
        public string IEN { get; set; }
        [Required, DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
