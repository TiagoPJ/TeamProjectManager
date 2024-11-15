using AutoMapper;
using Domain.Interface;
using Domain.Interface.Generic;
using Domain.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using Shared;
using Shared.DTO;

namespace TeamProjectManagerTest
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class ProjectServiceTests
    {
        private ProjectService _projectService;
        private Mock<IGenericProjectInterface> _genericProjectMock;
        private Mock<IGenericUserInterface> _genericUserMock;
        private Mock<ITaskHistoryService> _taskHistoryServiceMock;
        private Mock<IMapper> _mapperMock;
        private Mock<IOptions<Variables>> _optionsMock;

        #region Setup inicial

        [SetUp]
        public void SetUp()
        {
            _genericProjectMock = new();
            _genericUserMock = new();
            _taskHistoryServiceMock = new();
            _mapperMock = new();
            _optionsMock = new();
            _optionsMock.Setup(o => o.Value).Returns(new Variables { MaxTasks = 20 });

            _projectService = new ProjectService(
               _genericProjectMock.Object,
               _genericUserMock.Object,
               _optionsMock.Object,
               _taskHistoryServiceMock.Object,
               _mapperMock.Object);
        }

        #endregion

        [Test]
        public async Task AddProject_ValidProject_ReturnsCreated()
        {
            // Arrange
            var projectDto = new ProjectDto { Name = "Test Project", UserId = Guid.NewGuid() };
            var user = new ProjectUser { Id = projectDto.UserId };
            var project = new Project { Id = Guid.NewGuid() };

            _genericUserMock.Setup(u => u.GetEntityById(project.UserId)).ReturnsAsync(user);
            _mapperMock.Setup(m => m.Map<Project>(projectDto)).Returns(project);
            _genericProjectMock.Setup(p => p.Add(project)).Returns(Task.CompletedTask);

            // Act
            var result = await _projectService.AddProject(projectDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual("GetProject", createdResult.ActionName);
            Assert.AreEqual(project.Id, createdResult.RouteValues["id"]);
        }

        [Test]
        public async Task AddProject_ExceedMaxTasks_ReturnsBadRequest()
        {
            // Arrange
            var projectDto = new ProjectDto { Name = "Test Project", UserId = Guid.NewGuid() };
            var project = new Project { Id = Guid.NewGuid(), Tasks = Enumerable.Repeat(new ProjectTask(), _optionsMock.Object.Value.MaxTasks + 1).ToList() };
            _mapperMock.Setup(m => m.Map<Project>(projectDto)).Returns(project);

            // Act
            var result = await _projectService.AddProject(projectDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual($"Exceeded the maximum limit of {_optionsMock.Object.Value.MaxTasks} tasks.", badRequest.Value);
        }

        [Test]
        public async Task AddProject_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var projectDto = new ProjectDto { Name = "Test Project", UserId = Guid.NewGuid() };
            var project = new Project { Id = Guid.NewGuid() };
            _mapperMock.Setup(m => m.Map<Project>(projectDto)).Returns(project);

            // Act
            var result = await _projectService.AddProject(projectDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("User not found.", badRequest.Value);
        }

        [Test]
        public async Task DeleteProject_ProjectNotFound_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            // Act
            var result = await _projectService.DeleteProject(projectId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.AreEqual("Project not found.", badRequest.Value);
        }

        [Test]
        public async Task DeleteProject_ProjectHasPendingTasks_ReturnsBadRequest()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId, Tasks = new List<ProjectTask> { new ProjectTask { Status = Shared.Enums.Status.Pending } } };

            _genericProjectMock.Setup(p => p.GetProjectComplete(projectId)).ReturnsAsync(project);

            // Act
            var result = await _projectService.DeleteProject(projectId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.AreEqual("The project has pending tasks, you need to complete them or delete them.", badRequest.Value);
        }

        [Test]
        public async Task DeleteProject_SuccessfulDeletion_ReturnsNoContent()
        {
            // Arrange
            var projectId = Guid.NewGuid();
            var project = new Project { Id = projectId };

            _genericProjectMock.Setup(p => p.GetProjectComplete(projectId)).ReturnsAsync(project);
            _genericProjectMock.Setup(p => p.Delete(project)).Returns(Task.CompletedTask);

            // Act
            var result = await _projectService.DeleteProject(projectId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
