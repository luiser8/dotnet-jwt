using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace DotnetJWT.JWtUtils
{
    public static class JwtUtils
    {
        private static IConfiguration configuration;

        public static IConfiguration Configuration
        {
            get
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");
                configuration = builder.Build();
                return configuration;
            }
        }

        public static string GetSetting()
        {
            return Configuration.GetSection("AppSettings:Token").Value;
        }

        public static string CreateToken(TokenUserDto tokenUserDto)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, tokenUserDto.Id.ToString()),
                new Claim(ClaimTypes.Name, tokenUserDto.FirstName),
                new Claim(ClaimTypes.Surname, tokenUserDto.LastName),
                new Claim(ClaimTypes.Email, tokenUserDto.Email),
                new Claim(ClaimTypes.Role, tokenUserDto.RolUser.RolName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSetting()));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            var accessToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return accessToken;
        }

        public static string RefreshToken(TokenUserDto tokenUserDto)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, tokenUserDto.Id.ToString()),
                new Claim(ClaimTypes.Name, tokenUserDto.FirstName),
                new Claim(ClaimTypes.Surname, tokenUserDto.LastName),
                new Claim(ClaimTypes.Email, tokenUserDto.Email),
                new Claim(ClaimTypes.Role, tokenUserDto.RolUser.RolName),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(GetSetting()));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var jwt = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(7),
                signingCredentials: creds);

            var refreshToken = new JwtSecurityTokenHandler().WriteToken(jwt);

            return refreshToken;
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
