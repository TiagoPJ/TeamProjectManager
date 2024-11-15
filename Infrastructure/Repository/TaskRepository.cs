using Domain.Interface.Generic;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TaskRepository : GenericRepository<ProjectTask>, IGenericTaskInterface
    {
        private readonly DbContextOptions<ApplicationDbContext> _optionsBuilder;

        public TaskRepository()
        {
            _optionsBuilder = new DbContextOptions<ApplicationDbContext>();
        }

        public async Task<IEnumerable<ProjectTask>> GetTasks()
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.Tasks
                    .Include(t => t.Comments!)
                    .AsNoTracking().ToListAsync();
        }

        public async Task<ProjectTask> GetTask(Guid id)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.Tasks.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<ProjectTask?> GetTaskComplete(Guid id)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            var query = _context.Tasks
                    .Include(task => task.Comments!)
                    .Where(x => x.Id == id);

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByProject(Guid projectId)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            var query = _context.Tasks
                    .Include(task => task.Comments!)
                    .Where(x => x.ProjectId == projectId);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<IEnumerable<ProjectTask>> GetTasksByUser(Guid userId)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            var query = _context.Tasks
                    .Include(task => task.Comments!)
                    .Where(x => x.UserId == userId);

            return await query
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
