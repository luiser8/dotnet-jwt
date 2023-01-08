using DotnetJWT.Models;
using DotnetJWT.Repository;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;
using Microsoft.EntityFrameworkCore;

namespace DotnetJWT.Services
{
    public class UserService : IUserService
    {
        private readonly DbContextOptions<DBContext> _contextOptions;
        private readonly IUserRepository _userRepository;

        public UserService(
            DbContextOptions<DBContext> context,
            IUserRepository userRepository
        )
        {
            _contextOptions = context;
            _userRepository = userRepository;
        }

        public async Task<UserResponse> LoginUserService(LoginPayload loginPayload)
        {
            try
            {
                var response = await _userRepository.LoginUserRepository(loginPayload);
                return response;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<string> RefreshTokenService(string actualToken)
        {
            try
            {
                var response = await _userRepository.RefreshTokenRepository(actualToken);
                return response;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<UserResponse> PostUsersService(UserPayload userPayload)
        {
            try
            {
                bool userNameExists = await _userRepository.ByUserNameRepository(userPayload.UserName);
                bool userEmailExists = await _userRepository.ByEmailRepository(userPayload.Email);

                if (userNameExists)
                    throw new Exception("UserName all ready exists");

                if (userEmailExists)
                    throw new Exception("UserEmail all ready exists");

                var userCreated = await _userRepository.PostUsersRepository(userPayload);
                var userResponse = new UserResponse();

                if (userCreated != null)
                {
                    userResponse = new UserResponse
                    {
                        Id = userCreated.Id,
                        FirstName = userCreated.FirstName,
                        LastName = userCreated.LastName,
                        Email = userCreated.Email,
                        UserName = userCreated.UserName,
                        AccessToken = userCreated.AccessToken,
                        RefreshToken = userCreated.RefreshToken,
                        TokenCreated = userCreated.TokenCreated,
                        TokenExpires = userCreated.TokenExpires
                    };
                }

                return userResponse;
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
