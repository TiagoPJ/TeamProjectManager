using Entities.Entities;

namespace Domain.Interface.Generic
{
    public interface IGenericUserInterface : IGenericInterface<ProjectUser>
    {
        Task<IEnumerable<ProjectUser>> GetUsers();
        Task<ProjectUser> GetUser(Guid userId);
    }
}
