using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Domain.Interface
{
    public interface IProjectService
    {
        Task<ActionResult<Project>> AddProject(ProjectDto project);
        Task<IActionResult> DeleteProject(Guid id);
    }
}
