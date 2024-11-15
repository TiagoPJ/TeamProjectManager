using Infrastructure.Configuration;

namespace TeamProjectManager
{
    public static class TaskInitializer
    {
        public static WebApplication Seed(this WebApplication app)
        {
            using(var scope = app.Services.CreateScope())
            {
                using var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                context.Database.EnsureCreated();
                var users = context.Users.FirstOrDefault();

                if (users == null)
                {
                    context.Users.AddRange(
                        new Entities.Entities.ProjectUser() { Id = Guid.NewGuid(), Name = "Tiago Jesus", Position = Shared.Enums.Position.Manager },
                        new Entities.Entities.ProjectUser() { Id = Guid.NewGuid(), Name = "Alexa Soares", Position = Shared.Enums.Position.Simple },
                        new Entities.Entities.ProjectUser() { Id = Guid.NewGuid(), Name = "Adriana Mendes", Position = Shared.Enums.Position.Simple },
                        new Entities.Entities.ProjectUser() { Id = Guid.NewGuid(), Name = "Suzana Oliveira", Position = Shared.Enums.Position.Simple }
                        );

                    context.SaveChanges();
                }

                return app;
            }
        }
    }
}
