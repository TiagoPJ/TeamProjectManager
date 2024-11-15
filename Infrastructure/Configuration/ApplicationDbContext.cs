using Entities.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Configuration
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }

        public DbSet<ProjectUser> Users { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectTask> Tasks { get; set; }

        public DbSet<ProjectTaskComments> TaskComments { get; set; }

        public DbSet<ProjectTaskHistory> TaskHistorys { get; set; }

        public ApplicationDbContext() { }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(GetConnectionString());
                base.OnConfiguring(optionsBuilder);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }


        public string GetConnectionString()
        {
            return "Data Source=localhost, 1400;Initial Catalog=TeamProjectManagerDb;Integrated Security=False;User ID=sa;Password=Str0ngPa$$w0rd;Connect Timeout=15;Encrypt=False;TrustServerCertificate=False";
        }
    }
}
