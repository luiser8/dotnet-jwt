namespace DotnetJWT
{
    public class TokenUserDto
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public RolUser RolUser { get; set; }
    }

    public class RolUser
    {
        public int Id { get; set; }
        public string RolName { get; set; }
    }

    public class TokenResponseDto
    {
        public string accessToken { get; set; }
        public string refreshToken { get; set; }
    }
}