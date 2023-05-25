using MVVM_lb6.Domain.Models;
using Server.Services.Abstract;

namespace MVVM_lb6.Server.Helpers;

public static class AuthenticationHelper
{
    public static void ProvideSaltAndHash(this ApplicationUser user, IHashProvider hashProvider)
    {
        var salt = hashProvider.GenerateSalt();
        user.Salt = Convert.ToBase64String(salt);
        user.PasswordHash = hashProvider.ComputeHash(user.PasswordHash, user.Salt);
    }
}
