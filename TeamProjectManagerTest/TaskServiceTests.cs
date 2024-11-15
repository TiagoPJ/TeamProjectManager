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
    public class TaskServiceTests
    {
        private TaskService _taskService;
        private Mock<IGenericProjectInterface> _mockGenericProjectService;
        private Mock<IGenericTaskInterface> _mockGenericTaskInterface;
        private Mock<IGenericUserInterface> _mockGenericUserInterface;
        private Mock<ITaskHistoryService> _mockTaskHistoryInterface;
        private Mock<IMapper> _mockMapper;
        private Mock<IOptions<Variables>> _mockVariables;

        #region Setup inicial

        [SetUp]
        public void SetUp()
        {
            _mockGenericProjectService = new Mock<IGenericProjectInterface>();
            _mockGenericTaskInterface = new Mock<IGenericTaskInterface>();
            _mockGenericUserInterface = new Mock<IGenericUserInterface>();
            _mockTaskHistoryInterface = new Mock<ITaskHistoryService>();
            _mockMapper = new Mock<IMapper>();
            _mockVariables = new Mock<IOptions<Variables>>();
            _mockVariables.Setup(o => o.Value).Returns(new Variables { MaxTasks = 20 });

            _taskService = new TaskService(
               _mockGenericProjectService.Object,
               _mockGenericTaskInterface.Object,
               _mockVariables.Object,
               _mockTaskHistoryInterface.Object,
               _mockMapper.Object,
               _mockGenericUserInterface.Object);
        }

        #endregion

        [Test]
        public async Task AddTask_ValidTask_ReturnsCreated()
        {
            // Arrange
            var taskDto = new TaskDto { ProjectId = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "Test Task" };
            var user = new ProjectUser { Id = taskDto.UserId };
            var project = new Project { Id = taskDto.ProjectId, Tasks = new List<ProjectTask>() };

            _mockGenericProjectService.Setup(p => p.GetProjectComplete(taskDto.ProjectId)).ReturnsAsync(project);
            _mockGenericUserInterface.Setup(u => u.GetEntityById(taskDto.UserId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<ProjectTask>(taskDto)).Returns(new ProjectTask());
            _mockGenericTaskInterface.Setup(t => t.Add(It.IsAny<ProjectTask>())).Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.AddTask(taskDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);

            var createdResult = result.Result as CreatedAtActionResult;
            Assert.AreEqual("GetTask", createdResult.ActionName);
            Assert.NotNull(createdResult.RouteValues["id"]);
        }

        [Test]
        public async Task AddTask_ProjectNotFound_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = new TaskDto { ProjectId = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "Test Task" };

            // Act
            var result = await _taskService.AddTask(taskDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("Project not found.", badRequest.Value);
        }

        [Test]
        public async Task AddTask_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = new TaskDto { ProjectId = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "Test Task" };

            _mockGenericProjectService.Setup(p => p.GetProjectComplete(taskDto.ProjectId)).ReturnsAsync(new Project());

            // Act
            var result = await _taskService.AddTask(taskDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("User not found.", badRequest.Value);
        }

        [Test]
        public async Task AddTask_ExceedsMaximumTasks_ReturnsBadRequest()
        {
            // Arrange
            var taskDto = new TaskDto { ProjectId = Guid.NewGuid(), UserId = Guid.NewGuid(), Title = "Test Task" };
            var user = new ProjectUser { Id = taskDto.UserId };
            var project = new Project { Id = Guid.NewGuid(), Tasks = Enumerable.Repeat(new ProjectTask(), _mockVariables.Object.Value.MaxTasks).ToList() };
            _mockGenericProjectService.Setup(p => p.GetProjectComplete(taskDto.ProjectId)).ReturnsAsync(project);
            _mockGenericUserInterface.Setup(u => u.GetEntityById(taskDto.UserId)).ReturnsAsync(user);

            // Act
            var result = await _taskService.AddTask(taskDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual($"Exceeded the maximum limit of {_mockVariables.Object.Value.MaxTasks} tasks for this project. {project.Name}", badRequest.Value);
        }

        [Test]
        public async Task UpdateTask_ValidUpdate_ReturnsCreatedAtAction()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDto { Title = "Updated Title", Description = "Updated Description" };
            var oldTask = new ProjectTask { Id = taskId, Title = "Old Title", Description = "Old Description" };
            var updatedTask = new ProjectTask { Id = taskId, Title = "Updated Title", Description = "Updated Description" };

            _mockGenericTaskInterface.Setup(t => t.GetTask(taskId)).ReturnsAsync(oldTask);
            _mockMapper.Setup(m => m.Map<ProjectTask>(It.IsAny<ProjectTask>())).Returns(updatedTask);
            _mockGenericTaskInterface.Setup(t => t.Update(updatedTask)).Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.UpdateTask(taskId, taskDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result);
            var createdResult = result as CreatedAtActionResult;
            Assert.AreEqual("GetTask", createdResult.ActionName);
            Assert.AreEqual(taskId, createdResult.RouteValues["id"]);
        }

        [Test]
        public async Task UpdateTask_TaskNotFound_ReturnsBadRequest()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDto { Title = "Updated Title", Description = "Updated Description" };

            _mockGenericTaskInterface.Setup(t => t.GetTask(taskId)).ReturnsAsync((ProjectTask)null);

            // Act
            var result = await _taskService.UpdateTask(taskId, taskDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.AreEqual("Task not found.", badRequest.Value);
        }

        [Test]
        public async Task UpdateTask_NoChanges_ReturnsBadRequest()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var taskDto = new TaskDto { Title = "Old Title", Description = "Old Description" };
            var oldTask = new ProjectTask { Id = taskId, Title = "Old Title", Description = "Old Description" };

            _mockGenericTaskInterface.Setup(t => t.GetTask(taskId)).ReturnsAsync(oldTask);

            // Act
            var result = await _taskService.UpdateTask(taskId, taskDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.AreEqual("The task needs to be different some informations to update.", badRequest.Value);
        }

        [Test]
        public async Task DeleteTask_ValidTask_ReturnsNoContent()
        {
            // Arrange
            var taskId = Guid.NewGuid();
            var task = new ProjectTask { Id = taskId };

            _mockGenericTaskInterface.Setup(t => t.GetTaskComplete(taskId)).ReturnsAsync(task);
            _mockGenericTaskInterface.Setup(t => t.Delete(task)).Returns(Task.CompletedTask);

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }

        [Test]
        public async Task DeleteTask_TaskNotFound_ReturnsBadRequest()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _mockGenericTaskInterface.Setup(t => t.GetTaskComplete(taskId)).ReturnsAsync((ProjectTask)null);

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result);
            var badRequest = result as BadRequestObjectResult;
            Assert.AreEqual("Task not found.", badRequest.Value);
        }
    }
}
