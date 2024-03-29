using DotnetJWT.Models;
using DotnetJWT.Request.User.Payloads;
using DotnetJWT.Responses;
using DotnetJWT.JWtUtils;
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

        public async Task<List<UserResponse>> GetUsersRepository()
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var users = await _context.Users.ToListAsync();
                    var userResponse = new List<UserResponse>();
                    foreach (var item in users)
                    {
                        userResponse.Add(new UserResponse
                        {
                            Id = item.Id,
                            RoleId = (int)item.RoleId,
                            FirstName = item.FirstName,
                            LastName = item.LastName,
                            Email = item.Email,
                            UserName = item.UserName,
                            AccessToken = item.AccessToken,
                            RefreshToken = item.RefreshToken,
                            TokenCreated = item.TokenCreated,
                            TokenExpires = item.TokenExpires,
                        });
                    }
                    return userResponse;
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

        public async Task<TokenResponseDto> LoginUserRepository(LoginPayload loginPayload)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var user = new UserResponse();
                    var response = await _context.Users.FirstOrDefaultAsync(
                                                                            x => x.UserName == loginPayload.UserName
                                                                            && x.PasswordHash == MD5Utils.Md5utils.GetMD5(loginPayload.Password));

                    if (response == null)
                        throw new Exception("User not found");

                    var rolesResponse = await _context.Roles.Where(x => x.Id == response.RoleId).FirstOrDefaultAsync();
                    var roles = new Role { Id = rolesResponse.Id, RoleName = rolesResponse.RoleName };

                    string newAccessToken = JwtUtils.CreateToken(new TokenUserDto
                    {
                        Id = response.Id,
                        FirstName = response.FirstName,
                        LastName = response.LastName,
                        Email = response.Email,
                        RolUser = new RolUser { Id = roles.Id, RolName = roles.RoleName }
                    });
                    string newRefreshToken = JwtUtils.RefreshToken(new TokenUserDto
                    {
                        Id = response.Id,
                        FirstName = response.FirstName,
                        LastName = response.LastName,
                        Email = response.Email,
                        RolUser = new RolUser { Id = rolesResponse.Id, RolName = rolesResponse.RoleName }
                    });
                    response.AccessToken = newAccessToken;
                    response.RefreshToken = newRefreshToken;

                    _context.Entry(response).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return new TokenResponseDto { accessToken = newAccessToken, refreshToken = newRefreshToken };
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

                    string passwordHashCreated = MD5Utils.Md5utils.GetMD5(userPayload.Password);

                    var user = new User
                    {
                        FirstName = userPayload.FirstName,
                        LastName = userPayload.LastName,
                        Email = userPayload.Email,
                        UserName = userPayload.UserName,
                        PasswordHash = passwordHashCreated,
                        AccessToken = "",
                        RefreshToken = "",
                        TokenCreated = DateTime.Now,
                        TokenExpires = DateTime.Now.AddDays(7),
                        RoleId = userPayload.RoleId,
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

        public async Task<TokenResponseDto> RefreshTokenRepository(string actualToken)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    var response = await _context.Users.FirstOrDefaultAsync(x => x.RefreshToken.Contains(actualToken));

                    if (response == null)
                        throw new Exception("Invalid Refresh Token");
                    if (response.TokenExpires < DateTime.Now)
                        throw new Exception("Token expired");

                    var rolesResponse = await _context.Roles.Where(x => x.Id == response.RoleId).FirstOrDefaultAsync();

                    string newAccessToken = JwtUtils.CreateToken(new TokenUserDto
                    {
                        Id = response.Id,
                        FirstName = response.FirstName,
                        LastName = response.LastName,
                        Email = response.Email,
                        RolUser = new RolUser { Id = rolesResponse.Id, RolName = rolesResponse.RoleName }
                    });
                    string newRefreshToken = JwtUtils.RefreshToken(new TokenUserDto
                    {
                        Id = response.Id,
                        FirstName = response.FirstName,
                        LastName = response.LastName,
                        Email = response.Email,
                        RolUser = new RolUser { Id = rolesResponse.Id, RolName = rolesResponse.RoleName }
                    });

                    response.AccessToken = newAccessToken;
                    response.RefreshToken = newRefreshToken;

                    _context.Entry(response).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return new TokenResponseDto { accessToken = newAccessToken, refreshToken = newRefreshToken };
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}