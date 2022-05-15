using Articles.Models.Feature.Login;
using Microsoft.AspNetCore.Components.Authorization;

namespace Articles.Client.Authentication;

public interface IAuthenticationService
{
    Task<UserLoginResponse> Login(UserLoginResponse userForAuthentication);
    Task Logout();
}
