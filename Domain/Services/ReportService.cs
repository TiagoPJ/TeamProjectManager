using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared;

namespace Domain.Services
{
    public class ReportService : ControllerBase, IReportService
    {
        private readonly IGenericProjectInterface _genericProjectInterface;
        private readonly IGenericTaskInterface _genericTaskInterface;
        private readonly IGenericTaskCommentInterface _genericTaskCommentInterface;
        private readonly IGenericUserInterface _genericUserInterface;
        private readonly IOptions<Variables> _variables;

        public ReportService(IOptions<Variables> variables, IGenericProjectInterface genericProjectInterface, IGenericTaskInterface genericTaskInterface, IGenericTaskCommentInterface genericTaskCommentInterface, IGenericUserInterface genericUserInterface)
        {
            _variables = variables;
            _genericProjectInterface = genericProjectInterface;
            _genericTaskInterface = genericTaskInterface;
            _genericTaskCommentInterface = genericTaskCommentInterface;
            _genericUserInterface = genericUserInterface;
        }

        public async Task<ActionResult<ProjectReportUser>> GetReportUsers(Guid userId)
        {
            var user = await _genericUserInterface.GetEntityById(userId);
            if (user is null)
                return BadRequest("User not found.");

            if (user.Position != Shared.Enums.Position.Manager)
                return Forbid("User does not have permission to get reports.");

            var reportUser = await ReturnUserToReport(user);
            return Ok(reportUser);
        }

        public async Task<ActionResult<IEnumerable<ProjectReportUser>>> GetReportAllUsersByManager(Guid userId)
        {
            var user = await _genericUserInterface.GetEntityById(userId);
            if (user is null)
                return BadRequest("User not found.");

            if (user.Position != Shared.Enums.Position.Manager)
                return Forbid("User does not have permission to get reports.");

            var usersApp = await _genericUserInterface.List();
            var usersToReport = Enumerable.Empty<ProjectReportUser>();

            foreach (var userBase in usersApp)
            {
                usersToReport = usersToReport.Append(await ReturnUserToReport(userBase));
            }

            return Ok(usersToReport);
        }

        private async Task<ProjectReportUser> ReturnUserToReport(ProjectUser user)
        {
            var projects = await _genericProjectInterface.GetProjectsByUser(user.Id);
            var tasks = await _genericTaskInterface.GetTasksByUser(user.Id);
            var comments = await _genericTaskCommentInterface.GetCommentsByUser(user.Id);
            int qtdDaysConfigured = _variables.Value.DaysToReport;

            return new ProjectReportUser()
            {
                Name = user.Name,
                Position = user.Position.ToString(),
                QtdDays = qtdDaysConfigured,
                QtdProjects = projects.Where(x => x.CreateDate <= DateTime.Now.AddDays(qtdDaysConfigured)).Count(),
                QtdTasks = tasks.Where(x => x.CreateDate <= DateTime.Now.AddDays(qtdDaysConfigured)).Count(),
                QtdComments = comments.Where(x => x.CreateDate <= DateTime.Now.AddDays(qtdDaysConfigured)).Count()
            };
        }
    }
}
