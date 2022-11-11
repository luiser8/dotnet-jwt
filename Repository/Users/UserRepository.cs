using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;
using Microsoft.EntityFrameworkCore;

namespace DotnetJWT.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContextOptions<DBContext> _contextOptions;
        public UserRepository(DbContextOptions<DBContext> context)
        {
            _contextOptions = context;
        }

        public async Task<List<UserResponse>> GetUsers()
        {
            try{
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.ToListAsync();
                    var users = new List<UserResponse>();

                    return users;
                }
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<UserResponse> GetUser(int id)
        {
            try{
            using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
                    var user = new UserResponse
                    {
                        Id = response.Id,
                        FirstName = response.FirstName,
                        LastName = response.LastName,
                        Email = response.Email,
                        UserName = response.UserName,
                        Password = response.Password,
                        AccessToken = response.AccessToken,
                        RefreshToken = response.RefreshToken,
                        CreationDate = response.CreationDate
                    };
                    return user;
                }
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }
        public async Task<User> PostUsers(UserPayload userPayload)
        {
            try{
                using (var _context = new DBContext(_contextOptions))
                {
                    var user = new User
                    {
                        FirstName = userPayload.FirstName,
                        LastName = userPayload.LastName,
                        Email = userPayload.Email,
                        UserName = userPayload.UserName,
                        Password = userPayload.Password,
                        AccessToken = userPayload.AccessToken,
                        RefreshToken = userPayload.RefreshToken,
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    return user;
                }
            }catch(DbUpdateException ex){
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<User> PutUsers(int id, User user)
        {
            try{
                using (var _context = new DBContext(_contextOptions))
                {
                    _context.Entry(user).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    return user;
                }
            }catch(DbUpdateConcurrencyException ex){
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<User> DeleteUsers(int id)
        {
            try{
                using (var _context = new DBContext(_contextOptions))
                {
                    var user = await _context.Users.FindAsync(id);
                    if(user == null)
                    {
                        throw new NotImplementedException("User not exists");
                    }
                    _context.Entry(user).State = EntityState.Deleted;
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                    return user;
                }
            }catch(Exception ex){
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}