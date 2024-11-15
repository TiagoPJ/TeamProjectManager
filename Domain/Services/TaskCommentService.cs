using AutoMapper;
using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Domain.Services
{
    public class TaskCommentService : ControllerBase, ITaskCommentService
    {
        private readonly IGenericTaskInterface _genericTaskInterface;
        private readonly IGenericUserInterface _genericUserInterface;
        private readonly IGenericTaskCommentInterface _genericTaskCommentInterface;
        private readonly ITaskHistoryService _taskHistoryInterface;
        private readonly IMapper _mapper;

        public TaskCommentService(IGenericTaskInterface genericTaskInterface, ITaskHistoryService taskHistoryInterface, IMapper mapper, IGenericUserInterface genericUserInterface, IGenericTaskCommentInterface genericTaskCommentInterface)
        {
            _genericTaskInterface = genericTaskInterface;
            _taskHistoryInterface = taskHistoryInterface;
            _mapper = mapper;
            _genericUserInterface = genericUserInterface;
            _genericTaskCommentInterface = genericTaskCommentInterface;
        }

        public async Task<ActionResult<ProjectTaskComments>> AddTaskComment(TaskCommentsDto taskComment)
        {
            var task = await _genericTaskInterface.GetTask(taskComment.TaskId);
            if (task is null)
                return BadRequest("Task not found.");

            var user = await _genericUserInterface.GetEntityById(taskComment.UserId);
            if (user is null)
                return BadRequest("User not found.");

            var projecTaskCommentMapper = _mapper.Map<ProjectTaskComments>(taskComment);

            await _genericTaskCommentInterface.Add(projecTaskCommentMapper);
            await _taskHistoryInterface.AddTaskCommentHistory(task, projecTaskCommentMapper.Comment);

            return Ok(projecTaskCommentMapper);
        }
    }
}
