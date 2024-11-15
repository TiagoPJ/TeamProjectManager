using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace TeamProjectManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController
    {
        private readonly IUserService _userService;
        private readonly IGenericUserInterface _genericUserInterface;

        public UserController(IUserService userService, IGenericUserInterface genericUserInterface)
        {
            _userService = userService;
            _genericUserInterface = genericUserInterface;
        }

        [HttpGet("GetUsers")]
        [ProducesResponseType(typeof(IEnumerable<ProjectUser>), StatusCodes.Status200OK)]
        public async Task<object> GetUsers()
        {
            return await _genericUserInterface.GetUsers();
        }

        [HttpGet("{id}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectUser>), StatusCodes.Status200OK)]
        public async Task<object> GetUsers(Guid id)
        {
            return await _genericUserInterface.GetUser(id);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ProjectTask>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object> AddUser(ProjectUser user)
        {
            return await _userService.AddUser(user);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<ProjectTask>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            return await _userService.DeleteUser(id);
        }
    }
}
