using AutoMapper;
using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared;
using Shared.DTO;

namespace Domain.Services
{
    public class ProjectService : ControllerBase, IProjectService
    {
        private readonly IGenericProjectInterface _genericProjectService;
        private readonly IGenericUserInterface _genericUserInterface;
        private readonly ITaskHistoryService _taskHistoryInterface;
        private readonly IMapper _mapper;
        private readonly IOptions<Variables> _variables;

        public ProjectService(IGenericProjectInterface projectService, IGenericUserInterface genericUserInterface, IOptions<Variables> variables, ITaskHistoryService taskHistoryInterface, IMapper mapper)
        {
            _genericProjectService = projectService;
            _genericUserInterface = genericUserInterface;
            _variables = variables;
            _taskHistoryInterface = taskHistoryInterface;
            _mapper = mapper;
        }

        public async Task<ActionResult<Project>> AddProject(ProjectDto project)
        {
            var projectMapper = _mapper.Map<Project>(project);

            if (!VerifyQuantityTasks(projectMapper))
                return BadRequest($"Exceeded the maximum limit of {_variables.Value.MaxTasks} tasks.");

            var user = await _genericUserInterface.GetEntityById(projectMapper.UserId);
            if (user is null)
                return BadRequest("User not found.");

            await _genericProjectService.Add(projectMapper);

            if (projectMapper.Tasks is not null && projectMapper.Tasks.Any())
                await _taskHistoryInterface.AddTaskHistoryByList(projectMapper.Tasks);

            return CreatedAtAction("GetProject", new { id = projectMapper.Id, complete = true }, projectMapper);
        }

        public async Task<IActionResult> DeleteProject(Guid id)
        {
            var project = await _genericProjectService.GetProjectComplete(id);
            if (project is null)
                return BadRequest("Project not found.");

            var allTasksDone = project.Tasks is not null && project.Tasks.Any(x => x.Status == Shared.Enums.Status.Pending);
            if (allTasksDone)
                return BadRequest("The project has pending tasks, you need to complete them or delete them.");

            await _genericProjectService.Delete(project);

            return NoContent();
        }

        private bool VerifyQuantityTasks(Project project)
        {
            return project.Tasks is null || project.Tasks.Count() <= _variables.Value.MaxTasks;
        }
    }
}
