using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SharedLibrary.Models
{
	[Table("ApplicationUsers"), Serializable]
	public class ApplicationUser
	{
		[Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] 
		public Guid UserId { get; set; }
		public string Username { get; set; }
		public string PasswordHash { get; set; }
		public string Salt { get; set; }

		[NotNull] public string IndividualEmployeeNumber { get; set; } = "000-000-000";
	}
}

