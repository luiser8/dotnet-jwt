using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;

namespace DotnetJWT.Repository
{
    public interface IUserRepository
    {
        Task<UserResponse> LoginUserRepository(LoginPayload loginPayload);
        Task<User> PostUsersRepository(UserPayload userPayload);
        Task<string> RefreshTokenRepository(string actualToken);
        Task<bool> ByEmailRepository(string email);
        Task<bool> ByUserNameRepository(string userName);
    }
}