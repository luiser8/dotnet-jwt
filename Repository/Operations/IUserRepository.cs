using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;

namespace DotnetJWT.Repository
{
    public interface IUserRepository
    {
        Task<List<UserResponse>> GetUsers();
        Task<UserResponse> GetUser(int id);
        Task<User> PostUsers(UserPayload userPayload);
        Task<User> PutUsers(int id, User user);
        Task<User> DeleteUsers(int id);
    }
}