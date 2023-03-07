using DotnetJWT.Models;
using Microsoft.EntityFrameworkCore;

namespace DotnetJWT.Repository
{
    public class RolesRepository : IRolesRepository
    {
        private readonly DbContextOptions<DBContext> _contextOptions;
        public RolesRepository(DbContextOptions<DBContext> context)
        {
            _contextOptions = context;
        }

        public async Task<Role> GetRolesRepository(int RoleId)
        {
            try
            {
                using (var _context = new DBContext(_contextOptions))
                {
                    return await _context.Roles.Where(x => x.Id == RoleId).FirstOrDefaultAsync();
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException(ex.Message);
            }
        }
    }
}