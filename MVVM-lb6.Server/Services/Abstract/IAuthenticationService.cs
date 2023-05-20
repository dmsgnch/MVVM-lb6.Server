using Server.Domain;

namespace Server.Services.Abstract;

public interface IAuthenticationService
{
    AuthenticationResult Register(string username, string individualEmployeeNumber, string password);
    AuthenticationResult Login(string individualEmployeeNumber, string password);
}
