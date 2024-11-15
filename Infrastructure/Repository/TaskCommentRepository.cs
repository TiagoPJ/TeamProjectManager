using Domain.Interface.Generic;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class TaskCommentRepository : GenericRepository<ProjectTaskComments>, IGenericTaskCommentInterface
    {
        private readonly DbContextOptions<ApplicationDbContext> _optionsBuilder;

        public TaskCommentRepository()
        {
            _optionsBuilder = new DbContextOptions<ApplicationDbContext>();
        }

        public async Task<IEnumerable<ProjectTaskComments>> GetCommentsByUser(Guid userId)
        {
            using var _context = new ApplicationDbContext(_optionsBuilder);
            return await _context.TaskComments.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
        }
    }
}
