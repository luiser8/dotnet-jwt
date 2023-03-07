using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetJWT.Models
{
    [Table("Users")]
    public class User
    {
        public int Id { get; set; }
        public int? RoleId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }
        public virtual Role Roles { get; set; }
    }
}