using Microsoft.AspNetCore.Mvc;
using MVVM_lb6.Domain.Requests;
using MVVM_lb6.Domain.Responses;
using MVVM_lb6.Domain.Routes;
using Server.Domain;
using Server.Services.Abstract;

namespace Server.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
	private readonly IAuthenticationService _authenticationService;

	public AuthenticationController(IAuthenticationService authService)
	{
		_authenticationService = authService;
	}

	[HttpPost(ApiRoutes.Authentication.Register)]
	public IActionResult Register([FromBody] RegistrationRequest authRequest)
	{
		var result = _authenticationService.Register(authRequest.Username, authRequest.IndividualEmployeeNumber, authRequest.Password);
		if (result.IsSuccessful is false) 
			return BadRequest(new LambdaResponse() {Info = result.Message});

		return Ok(new LambdaResponse() {Info = result.Message});
	}
	
	[HttpPost(ApiRoutes.Authentication.Login)]
	public IActionResult Login([FromBody] LoginRequest request)
	{
        var result = _authenticationService.Login(request.IEN, request.Password);
        if (result.Success == false) 
	        return BadRequest(new AuthenticationResponse(result.OperationInfo));

        return Ok(new AuthenticationResponse(result.OperationInfo, result.AccessToken));
    }
}
