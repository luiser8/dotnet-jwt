using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;

namespace DotnetJWT.Services
{
    public interface IUserService
    {
        Task<List<UserResponse>> GetUsersService();
        Task<TokenResponseDto> LoginUserService(LoginPayload loginPayload);
        Task<UserResponse> PostUsersService(UserPayload userPayload);
        Task<TokenResponseDto> RefreshTokenService(string actualToken);
    }
}