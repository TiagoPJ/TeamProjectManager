using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using System.Net.Mime;

namespace TeamProjectManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TaskCommentController
    {
        private readonly ITaskCommentService _taskCommentService;

        public TaskCommentController(ITaskCommentService taskCommentService, IGenericTaskInterface projectInterfaceGeneric)
        {
            _taskCommentService = taskCommentService;
        }

        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ProjectTask>(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<object> AddComment(TaskCommentsDto taskComment)
        {
            return await _taskCommentService.AddTaskComment(taskComment);
        }
    }
}
