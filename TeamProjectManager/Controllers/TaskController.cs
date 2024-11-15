using Domain.Interface;
using Domain.Interface.Generic;
using Domain.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text.Json.Serialization;

namespace TeamProjectManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskController
    {
        private readonly ITaskService _taskService;
        private readonly IGenericTaskInterface _projectTaskInterfaceGeneric;

        public TaskController(ITaskService taskService, IGenericTaskInterface projectInterfaceGeneric)
        {
            _taskService = taskService;
            _projectTaskInterfaceGeneric = projectInterfaceGeneric;
        }

        [HttpGet("GetTasks")]
        [ProducesResponseType(typeof(IEnumerable<ProjectTask>), StatusCodes.Status200OK)]
        public async Task<object> GetTasks()
        {
            return await _projectTaskInterfaceGeneric.GetTasks();
        }

        [HttpGet("{id}/{complete}")]
        [ProducesResponseType<ProjectTask>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object?> GetTask(Guid id, bool complete = false)
        {
           return !complete ? await _projectTaskInterfaceGeneric.GetTask(id) : await _projectTaskInterfaceGeneric.GetTaskComplete(id);
        }

        [HttpGet("project/{projectId}")]
        [ProducesResponseType(typeof(IEnumerable<ProjectTask>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object> GetTasksByProject(Guid projectId)
        {
            return await _projectTaskInterfaceGeneric.GetTasksByProject(projectId);
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ProjectTask>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object> AddTask(TaskDto task)
        {
            return await _taskService.AddTask(task);
        }

        [HttpDelete("{id}")]
        [ProducesResponseType<ProjectTask>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteTask(Guid id)
        {
            return await _taskService.DeleteTask(id);
        }

        [HttpPut("{id}")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateTask(Guid id, [FromBody]TaskDto task)
        {
            return await _taskService.UpdateTask(id, task);
        }
    }
}
