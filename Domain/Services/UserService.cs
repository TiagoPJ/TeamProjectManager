using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Services
{
    public class UserService : ControllerBase, IUserService
    {
        private readonly IGenericUserInterface _genericUserInterface;

        public UserService(IGenericUserInterface genericUserInterface)
        {
            _genericUserInterface = genericUserInterface;
        }

        public async Task<ActionResult<ProjectUser>> AddUser(ProjectUser user)
        {
            await _genericUserInterface.Add(user);

            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        public async Task<IActionResult> DeleteUser(Guid userId)
        {
            var user = await _genericUserInterface.GetEntityById(userId);
            if (user is null)
                return BadRequest("User not found.");

            await _genericUserInterface.Delete(user);

            return NoContent();
        }
    }
}
