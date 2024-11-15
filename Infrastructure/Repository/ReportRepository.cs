using Domain.Interface.Generic;
using Entities.Entities;
using Infrastructure.Configuration;
using Infrastructure.Repository.Generic;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository
{
    public class ReportRepository : GenericRepository<ProjectReportUser>, IGenericReportInterface
    {
        private readonly DbContextOptions<ApplicationDbContext> _optionsBuilder;

        public ReportRepository()
        {
            _optionsBuilder = new DbContextOptions<ApplicationDbContext>();
        }
    }
}
