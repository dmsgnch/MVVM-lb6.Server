using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using MVVM_lb6.Domain.Models;
using MVVM_lb6.Domain.Responses;
using MVVM_lb6.Server.Helpers;
using MVVM_lb6.Server.Models;
using Newtonsoft.Json;
using Server.Common.Constants;
using Server.Domain;
using Server.Services.Abstract;

namespace Server.Services;

public class AuthenticationService : IAuthenticationService
{
	public Settings Settings { get; init; }
	public ApplicationDbContext Context { get; init; }
	private readonly IHashProvider _hashProvider;

    public AuthenticationService(Settings settings, ApplicationDbContext context, IHashProvider hashProvider)
    {
        Settings = settings;
        Context = context;
        _hashProvider = hashProvider;
    }

    public ServiceResult Register(string username, string individualEmployeeNumber, string password)
	{
		if(Context.Users.Any(u => u.IndividualEmployeeNumber.Equals(individualEmployeeNumber)))
			return new ServiceResult(ErrorMessages.User.AlreadyExists) {IsSuccessful = false};

		var user = new ApplicationUser
		{
			Username = username,
			PasswordHash = password,
			IndividualEmployeeNumber = individualEmployeeNumber
		};
		user.ProvideSaltAndHash(_hashProvider);

		Context.Add(user);
		Context.SaveChanges();

		return new ServiceResult(SuccessMessages.User.Created) {IsSuccessful = true};
	}
    
	public AuthenticationResult Login(string individualEmployeeNumber, string password)
	{
		var user = Context.Users.FirstOrDefault(u => 
			u.IndividualEmployeeNumber.Equals(individualEmployeeNumber));

		if (user == null ||
		    user.PasswordHash != _hashProvider.ComputeHash(password, user.Salt)) 
			return new AuthenticationResult(new [] {ErrorMessages.Authentication.NotRecordsFound});

		return new AuthenticationResult(new [] {SuccessMessages.Authentication.SuccessfulLogin}, 
			GenerateJwtToken(AssembleClaimsIdentity(user)));
	}

	private ClaimsIdentity AssembleClaimsIdentity(ApplicationUser user)
	{
		var subject = new ClaimsIdentity(new[]
		{
			new Claim("id", user.UserId.ToString())
		});

		return subject;
	}
	private string GenerateJwtToken(ClaimsIdentity subject)
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var key = Encoding.ASCII.GetBytes(Settings.BearerKey);
		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = subject,
			Expires = DateTime.Now.AddYears(10),
			SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
		};
		var token = tokenHandler.CreateToken(tokenDescriptor);
		return tokenHandler.WriteToken(token);
	}
}