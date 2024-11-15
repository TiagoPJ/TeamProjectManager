using Entities.Entities;

namespace Domain.Interface
{
    public interface ITaskHistoryService
    {
        Task AddTaskHistoryByList(IEnumerable<ProjectTask> tasks);
        Task AddTaskHistory(ProjectTask task, ProjectTask oldTask = default);
        Task AddTaskCommentHistory(ProjectTask task, string comment);
    }
}
