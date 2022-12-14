using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;
using DotnetJWT.JWtUtils;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DotnetJWT.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly DbContextOptions<DBContext> _contextOptions;
        public UserRepository(DbContextOptions<DBContext> context)
        {
            _contextOptions = context;
        }

        public async Task<List<User>> GetUsersRepository()
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.ToListAsync();
                    return response;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<bool> ByEmailRepository(string email)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.FirstOrDefaultAsync(x => x.Email == email);
                    return response == null ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<bool> ByUserNameRepository(string userName)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.FirstOrDefaultAsync(x => x.UserName == userName);
                    return response == null ? false : true;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<UserResponse?> LoginUserRepository(LoginPayload loginPayload)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginPayload.UserName);
                    var user = new UserResponse();

                    if (response == null)
                        throw new Exception("User not found");

                    byte[] bytesPasswordHash = Encoding.ASCII.GetBytes(response.PasswordHash);
                    byte[] bytesPasswordSalt = Encoding.ASCII.GetBytes(response.PasswordSalt);

                    if (JwtUtils.VerifyPasswordHash(response.PasswordHash, bytesPasswordHash, bytesPasswordSalt))
                    {
                        return user;
                    }

                    if (response != null)
                    {
                        user = new UserResponse
                        {
                            Id = response.Id,
                            FirstName = response.FirstName,
                            LastName = response.LastName,
                            Email = response.Email,
                            UserName = response.UserName,
                            AccessToken = response.AccessToken,
                            RefreshToken = response.RefreshToken,
                            TokenCreated = response.TokenCreated,
                            TokenExpires = response.TokenExpires
                        };
                    }

                    string newAccessToken = JwtUtils.CreateToken(response.UserName);
                    response.AccessToken = newAccessToken;

                    _context.Entry(response).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return user;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<User> PostUsersRepository(UserPayload userPayload)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    JwtUtils.CreatePasswordHash(userPayload.Password, out byte[] passwordHash, out byte[] passwordSalt);
                    string accessToken = JwtUtils.CreateToken(userPayload.UserName);
                    string refreshToken = JwtUtils.RefreshToken(userPayload.UserName);
                    string passwordHashCreated = Convert.ToBase64String(passwordHash);
                    string passwordSaltCreated = Convert.ToBase64String(passwordSalt);

                    var user = new User
                    {
                        FirstName = userPayload.FirstName,
                        LastName = userPayload.LastName,
                        Email = userPayload.Email,
                        UserName = userPayload.UserName,
                        PasswordHash = passwordHashCreated,
                        PasswordSalt = passwordSaltCreated,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        TokenCreated = DateTime.Now,
                        TokenExpires = DateTime.Now.AddDays(7),
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();

                    return user;
                }
            }
            catch (DbUpdateException ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }

        public async Task<string> RefreshTokenRepository(string actualToken)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken.Contains(actualToken));

                    if (response == null)
                        return "Invalid Refresh Token";
                    if (response.TokenExpires < DateTime.Now)
                        return "Token expired";

                    string newAccessToken = JwtUtils.CreateToken(response.UserName);
                    string newRefreshToken = JwtUtils.RefreshToken(response.UserName);

                    response.AccessToken = newAccessToken;
                    response.RefreshToken = newRefreshToken;

                    _context.Entry(response).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return newAccessToken;
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}