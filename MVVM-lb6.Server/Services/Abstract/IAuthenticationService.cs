using MVVM_lb6.Domain.Responses;
using Server.Domain;

namespace Server.Services.Abstract;

public interface IAuthenticationService
{
    ServiceResult Register(string username, string individualEmployeeNumber, string password);
    AuthenticationResult Login(string individualEmployeeNumber, string password);
}
