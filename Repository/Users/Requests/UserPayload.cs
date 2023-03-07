using DotnetJWT.Models;

namespace DotnetJWT.Request.User.Payloads
{
    public class UserPayload
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public int? RoleId { get; set; }
    }
}