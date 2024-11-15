using Entities.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Domain.Interface
{
    public interface IReportService
    {
        Task<ActionResult<ProjectReportUser>> GetReportUsers(Guid userId);
        Task<ActionResult<IEnumerable<ProjectReportUser>>> GetReportAllUsersByManager(Guid userId);        
    }
}
