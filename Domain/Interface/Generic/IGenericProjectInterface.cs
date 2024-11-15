using Entities.Entities;

namespace Domain.Interface.Generic
{
    public interface IGenericProjectInterface : IGenericInterface<Project>
    {
        Task<IEnumerable<Project>> GetProjects();
        Task<Project> GetProject(Guid id);
        Task<Project?> GetProjectComplete(Guid id);
        Task<IEnumerable<Project>> GetProjectsByUser(Guid userId);        
    }
}
