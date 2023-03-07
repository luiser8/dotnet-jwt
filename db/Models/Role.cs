using System.ComponentModel.DataAnnotations.Schema;

namespace DotnetJWT.Models
{
    [Table("Roles")]
    public class Role
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }
    }
}