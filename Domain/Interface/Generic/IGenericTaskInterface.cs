using Entities.Entities;

namespace Domain.Interface.Generic
{
    public interface IGenericTaskInterface : IGenericInterface<ProjectTask>
    {
        Task<IEnumerable<ProjectTask>> GetTasks();
        Task<ProjectTask> GetTask(Guid id);
        Task<ProjectTask?> GetTaskComplete(Guid id);
        Task<IEnumerable<ProjectTask>> GetTasksByProject(Guid projectId);
        Task<IEnumerable<ProjectTask>> GetTasksByUser(Guid userId);
    }
}
