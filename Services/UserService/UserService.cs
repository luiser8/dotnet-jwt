using DotnetJWT.Models;
using DotnetJWT.Repository;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

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

        public async Task<List<UserResponse>> GetUsers()
        {
            try{
                var response = await _userRepository.GetUsers();
                return response;
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }
        public async Task<UserResponse> GetUser(int id)
        {
            try{
                var user = await _userRepository.GetUser(id);

                return user;
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }
        public async Task<User> PostUsers(UserPayload userPayload)
        {
            try{
                var userCreated = await _userRepository.PostUsers(userPayload);

                return userCreated;
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<User> PutUsers(int id, User user)
        {
            try{
                var response = await _userRepository.PutUsers(id, user);
                return response;
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }
        public async Task<User> DeleteUsers(int id)
        {
            try{
                var response = await _userRepository.DeleteUsers(id);
                return response;
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}
