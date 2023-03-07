using DotnetJWT.Models;

namespace DotnetJWT.Repository
{
    public interface IRolesRepository
    {
        Task<Role> GetRolesRepository(int RoleId);
    }
}