using Domain.Interface.Generic;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class UserRepository : GenericRepository<ProjectUser>, IGenericUserInterface
    {
        private readonly DbContextOptions<ApplicationDbContext> _optionsBuilder;

        public UserRepository()
        {
            _optionsBuilder = new DbContextOptions<ApplicationDbContext>();
        }
        public async Task<IEnumerable<ProjectUser>> GetUsers()
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.Users.AsNoTracking().ToListAsync();
        }

        public async Task<ProjectUser> GetUser(Guid userId)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(userId));
        }
    }
}
