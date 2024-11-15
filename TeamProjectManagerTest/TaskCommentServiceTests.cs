using AutoMapper;
using Domain.Interface;
using Domain.Interface.Generic;
using Domain.Services;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shared.DTO;

namespace TeamProjectManagerTest
{
    [Parallelizable(ParallelScope.Self)]
    [TestFixture]
    public class TaskCommentServiceTests
    {
        private Mock<IGenericTaskInterface> _mockGenericTaskInterface;
        private Mock<IGenericUserInterface> _mockGenericUserInterface;
        private Mock<IGenericTaskCommentInterface> _mockGenericTaskCommentInterface;
        private Mock<ITaskHistoryService> _mockTaskHistoryInterface;
        private Mock<IMapper> _mockMapper;
        private TaskCommentService _taskCommentService;

        #region Setup inicial

        [SetUp]
        public void SetUp()
        {
            _mockGenericTaskInterface = new Mock<IGenericTaskInterface>();
            _mockGenericUserInterface = new Mock<IGenericUserInterface>();
            _mockGenericTaskCommentInterface = new Mock<IGenericTaskCommentInterface>();
            _mockTaskHistoryInterface = new Mock<ITaskHistoryService>();
            _mockMapper = new Mock<IMapper>();

            _taskCommentService = new TaskCommentService(
                _mockGenericTaskInterface.Object,
                _mockTaskHistoryInterface.Object,
                _mockMapper.Object,
                _mockGenericUserInterface.Object,
                _mockGenericTaskCommentInterface.Object);
        }

        #endregion

        [Test]
        public async Task AddTaskComment_ValidComment_ReturnsOk()
        {
            // Arrange
            var taskCommentDto = new TaskCommentsDto { TaskId = Guid.NewGuid(), UserId = Guid.NewGuid(), Comment = "Test Comment" };
            var user = new ProjectUser { Id = taskCommentDto.UserId };
            var task = new ProjectTask { Id = taskCommentDto.TaskId };

            _mockGenericTaskInterface.Setup(t => t.GetTask(taskCommentDto.TaskId)).ReturnsAsync(task);
            _mockGenericUserInterface.Setup(u => u.GetEntityById(taskCommentDto.UserId)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<ProjectTaskComments>(taskCommentDto)).Returns(new ProjectTaskComments());
            _mockGenericTaskCommentInterface.Setup(c => c.Add(It.IsAny<ProjectTaskComments>())).Returns(Task.CompletedTask);

            // Act
            var result = await _taskCommentService.AddTaskComment(taskCommentDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.NotNull(okResult.Value);
        }

        [Test]
        public async Task AddTaskComment_TaskNotFound_ReturnsBadRequest()
        {
            // Arrange
            var taskCommentDto = new TaskCommentsDto { TaskId = Guid.NewGuid(), UserId = Guid.NewGuid(), Comment = "Test Comment" };

            // Act
            var result = await _taskCommentService.AddTaskComment(taskCommentDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("Task not found.", badRequest.Value);
        }

        [Test]
        public async Task AddTaskComment_UserNotFound_ReturnsBadRequest()
        {
            // Arrange
            var taskCommentDto = new TaskCommentsDto { TaskId = Guid.NewGuid(), UserId = Guid.NewGuid(), Comment = "Test Comment" };
            var task = new ProjectTask { Id = taskCommentDto.TaskId };

            _mockGenericTaskInterface.Setup(t => t.GetTask(taskCommentDto.TaskId)).ReturnsAsync(task);

            // Act
            var result = await _taskCommentService.AddTaskComment(taskCommentDto);

            // Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
            var badRequest = result.Result as BadRequestObjectResult;
            Assert.AreEqual("User not found.", badRequest.Value);
        }
    }
}
