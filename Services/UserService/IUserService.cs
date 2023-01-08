using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;

namespace DotnetJWT.Services
{
    public interface IUserService
    {
        Task<UserResponse> LoginUserService(LoginPayload loginPayload);
        Task<UserResponse> PostUsersService(UserPayload userPayload);
        Task<string> RefreshTokenService(string actualToken);
    }
}