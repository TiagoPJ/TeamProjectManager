using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;

namespace Domain.Interface
{
    public interface ITaskCommentService
    {
        Task<ActionResult<ProjectTaskComments>> AddTaskComment(TaskCommentsDto comment);
    }
}
