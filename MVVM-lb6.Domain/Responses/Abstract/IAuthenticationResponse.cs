using System;
using System.Collections.Generic;
using System.Text;

namespace MVVM_lb6.Domain.Responses.Abstract
{
	public interface IAuthenticationResponse
	{
		public string? Token { get; set; }
	}
}
