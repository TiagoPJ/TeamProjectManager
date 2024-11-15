using Domain.Interface.Generic;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ProjectRepository : GenericRepository<Project>, IGenericProjectInterface
    {
        private readonly DbContextOptions<ApplicationDbContext> _optionsBuilder;

        public ProjectRepository()
        {
            _optionsBuilder = new DbContextOptions<ApplicationDbContext>();
        }

        public async Task<IEnumerable<Project>> GetProjects()
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.Projects
                    .Include(x => x.Tasks!)
                    .ThenInclude(t => t.Comments!)
                    .AsNoTracking().ToListAsync();
        }

        public async Task<Project> GetProject(Guid id)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.Projects.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Project?> GetProjectComplete(Guid id)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            var query = _context.Projects
                    .Include(x => x.Tasks!)
                    .ThenInclude(task => task.Comments!)
                    .Where(x => x.Id == id);

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Project>> GetProjectsByUser(Guid userId)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            var query = _context.Projects
                    .Include(x => x.Tasks!)
                    .ThenInclude(task => task.Comments!)
                    .Where(x => x.UserId == userId);

            return await query
                .AsNoTracking().ToListAsync();
        }
    }
}
