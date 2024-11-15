using Domain.Interface;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace TeamProjectManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportController
    {
        private readonly IReportService _reportService;

        public ReportController(IReportService reportService)
        {
            _reportService = reportService;
        }

        [HttpGet("GetReportManager/{userId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ProjectReportUser>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<object> GetReport(Guid userId)
        {
            return await _reportService.GetReportUsers(userId);
        }

        [HttpGet("GetReportAllUsersByManager/{userId}")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType<ProjectReportUser>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<object> GetReportAllUsersByManager(Guid userId)
        {
            return await _reportService.GetReportAllUsersByManager(userId);
        }
    }
}
