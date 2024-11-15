using Domain.Interface.Generic;
using Domain.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Shared;

namespace TeamProjectManagerTest
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ReportServiceTests
    {
        private Mock<IOptions<Variables>> _mockVariables;
        private Mock<IGenericProjectInterface> _mockGenericProjectInterface;
        private Mock<IGenericTaskInterface> _mockGenericTaskInterface;
        private Mock<IGenericTaskCommentInterface> _mockGenericTaskCommentInterface;
        private Mock<IGenericUserInterface> _mockGenericUserInterface;
        private ReportService _reportService;

        public ReportServiceTests()
        {
            _mockVariables = new Mock<IOptions<Variables>>();
            _mockVariables.Setup(o => o.Value).Returns(new Variables { MaxTasks = 20, DaysToReport = 30 });
            _mockGenericProjectInterface = new Mock<IGenericProjectInterface>();
            _mockGenericTaskInterface = new Mock<IGenericTaskInterface>();
            _mockGenericTaskCommentInterface = new Mock<IGenericTaskCommentInterface>();
            _mockGenericUserInterface = new Mock<IGenericUserInterface>();
            _reportService = new ReportService(
                _mockVariables.Object,
                _mockGenericProjectInterface.Object,
                _mockGenericTaskInterface.Object,
                _mockGenericTaskCommentInterface.Object,
                _mockGenericUserInterface.Object);
        }

        [Test]
        public async Task GetReportUsers_ValidManager_ReturnsOk()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ProjectUser { Id = userId, Position = Shared.Enums.Position.Manager };
            var projects = new List<Project>();
            var tasks = new List<ProjectTask>();
            var comments = new List<ProjectTaskComments>();

            _mockGenericUserInterface.Setup(u => u.GetEntityById(userId)).ReturnsAsync(user);
            _mockGenericProjectInterface.Setup(p => p.GetProjectsByUser(userId)).ReturnsAsync(projects);
            _mockGenericTaskInterface.Setup(t => t.GetTasksByUser(userId)).ReturnsAsync(tasks);
            _mockGenericTaskCommentInterface.Setup(c => c.GetCommentsByUser(userId)).ReturnsAsync(comments);

            // Act
            var result = await _reportService.GetReportUsers(userId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult.Value);
            Assert.IsInstanceOf<ProjectReportUser>(okResult.Value);
        }

        [Test]
        public async Task GetReportUsers_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var userId = Guid.NewGuid();

            // Act
            var result = await _reportService.GetReportUsers(userId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("User not found.", badRequest.Value);
        }

        [Test]
        public async Task GetReportUsers_UserNotManager_ReturnsForbid()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var user = new ProjectUser { Id = userId, Position = Shared.Enums.Position.Simple };

            _mockGenericUserInterface.Setup(u => u.GetEntityById(userId)).ReturnsAsync(user);

            // Act
            var result = await _reportService.GetReportUsers(userId);

            // Assert
            Assert.IsInstanceOf<ForbidResult>(result.Result);
        }
    }
}
