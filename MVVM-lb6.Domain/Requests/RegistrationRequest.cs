using System;
using System.ComponentModel.DataAnnotations;

namespace MVVM_lb6.Domain.Requests
{
	[Serializable]
	public class RegistrationRequest
	{
		[Required]
		public string Username { get; set; } = "";
		
		[Required]
		public string IndividualEmployeeNumber { get; set; } = "";
		
		[Required, DataType(DataType.Password)]
		public string Password { get; set; } = "";
	}
}