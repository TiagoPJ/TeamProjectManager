using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Interface
{
    public interface IUserService
    {
        Task<ActionResult<ProjectUser>> AddUser(ProjectUser user);
        Task<IActionResult> DeleteUser(Guid id);
    }
}
