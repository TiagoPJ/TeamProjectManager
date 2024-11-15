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
    public class TaskService : ControllerBase, ITaskService
    {
        private readonly IGenericProjectInterface _genericProjectService;
        private readonly IGenericTaskInterface _genericTaskInterface;
        private readonly IGenericUserInterface _genericUserInterface;
        private readonly ITaskHistoryService _taskHistoryInterface;
        private readonly IMapper _mapper;
        private readonly IOptions<Variables> _variables;

        public TaskService(IGenericProjectInterface projectService, IGenericTaskInterface genericTaskInterface, IOptions<Variables> variables, ITaskHistoryService taskHistoryInterface, IMapper mapper, IGenericUserInterface genericUserInterface)
        {
            _genericProjectService = projectService;
            _genericTaskInterface = genericTaskInterface;
            _variables = variables;
            _taskHistoryInterface = taskHistoryInterface;
            _mapper = mapper;
            _genericUserInterface = genericUserInterface;
        }

        public async Task<ActionResult<ProjectTask>> AddTask(TaskDto task)
        {
            var project = await _genericProjectService.GetProjectComplete(task.ProjectId);
            if (project is null)
                return BadRequest("Project not found.");

            var user = await _genericUserInterface.GetEntityById(task.UserId);
            if (user is null)
                return BadRequest("User not found.");

            if (project.Tasks?.Count() == _variables.Value.MaxTasks)
                return BadRequest($"Exceeded the maximum limit of {_variables.Value.MaxTasks} tasks for this project. {project.Name}");

            var projecTasktMapper = _mapper.Map<ProjectTask>(task);

            await _genericTaskInterface.Add(projecTasktMapper);
            await _taskHistoryInterface.AddTaskHistory(projecTasktMapper);

            return CreatedAtAction("GetTask", new { id = projecTasktMapper.Id, complete = true }, projecTasktMapper);
        }

        public async Task<IActionResult> UpdateTask(Guid id, TaskDto task)
        {
            var taskBase = await _genericTaskInterface.GetTask(id);
            if (taskBase is null)
                return BadRequest("Task not found.");

            if (!VerifyModificationTask(task, taskBase))
                return BadRequest("The task needs to be different some informations to update.");

            var oldTask = taskBase.Clone();
            taskBase.Status = task.Status;
            taskBase.Title = task.Title;
            taskBase.Description = task.Description;
            taskBase.ExpirationDate = task.ExpirationDate;

            var projecTasktMapper = _mapper.Map<ProjectTask>(taskBase);

            await _genericTaskInterface.Update(projecTasktMapper);
            await _taskHistoryInterface.AddTaskHistory(projecTasktMapper, (ProjectTask)oldTask);

            return CreatedAtAction("GetTask", new { id = projecTasktMapper.Id, complete = true }, projecTasktMapper);
        }

        public async Task<IActionResult> DeleteTask(Guid id)
        {
            var task = await _genericTaskInterface.GetTaskComplete(id);
            if (task is null)
                return BadRequest("Task not found.");

            await _genericTaskInterface.Delete(task);

            return NoContent();
        }

        private static bool VerifyModificationTask(TaskDto task, ProjectTask oldTask)
        {
            if (task.Status == oldTask.Status &&
               task.ExpirationDate == oldTask.ExpirationDate &&
               task.Title == oldTask.Title &&
               task.Description == oldTask.Description)
                return false;

            return true;
        }
    }
}
