using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace TeamProjectManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProjectController
    {
        private readonly IProjectService _projectService;
        private readonly IGenericProjectInterface _projectInterfaceGeneric;

        public ProjectController(IProjectService projectService, IGenericProjectInterface projectInterfaceGeneric)
        {
            _projectService = projectService;
            _projectInterfaceGeneric = projectInterfaceGeneric;
        }

        [HttpGet("GetProjects")]
        [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
        public async Task<object> GetProjects()
        {
            return await _projectInterfaceGeneric.GetProjects();
        }

        [HttpGet("{id}/{complete}")]
        [ProducesResponseType<Project>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object?> GetProject(Guid id, bool complete = false)
        {
           return !complete ? await _projectInterfaceGeneric.GetProject(id) : await _projectInterfaceGeneric.GetProjectComplete(id);
        }

        [HttpGet("user/{userId}")]
        [ProducesResponseType(typeof(IEnumerable<Project>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<object> GetProjectByUser(Guid userId)
        {
            return await _projectInterfaceGeneric.GetProjectsByUser(userId);
        }

        [HttpPost]
        [ProducesResponseType<Project>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<object> AddProject(ProjectDto project)
        {
            return await _projectService.AddProject(project);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<Project>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProject(Guid id)
        {
            return await _projectService.DeleteProject(id);
        }
    }
}
