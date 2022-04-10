using Articles.Models.Feature.Login;

namespace Articles.Client.Authentication;

public interface IAuthenticationService
{
    Task<UserLoginResponse> Login(UserLoginResponse userForAuthentication);
    Task Logout();
}
