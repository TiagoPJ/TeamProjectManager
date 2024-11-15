using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Domain.Interface
{
    public interface ITaskService
    {
        Task<ActionResult<ProjectTask>> AddTask(TaskDto task);
        Task<IActionResult> DeleteTask(Guid id);
        Task<IActionResult> UpdateTask(Guid id, TaskDto task);
    }
}
