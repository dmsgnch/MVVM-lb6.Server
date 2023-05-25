using System;
using System.Collections.Generic;
using System.Linq;
using MVVM_lb6.Domain.Responses.Abstract;

namespace MVVM_lb6.Domain.Responses
{
	[Serializable]
	public class AuthenticationResponse : ResponseBase, IAuthenticationResponse
	{
		public string? Token { get; set; }

		public AuthenticationResponse()
		{ }

		public AuthenticationResponse(IEnumerable<string> info, string? token = null)
		{
			Info = info.ToArray();
			Token = token;
		}
	}
}


