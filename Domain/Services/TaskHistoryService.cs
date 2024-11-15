using Domain.Interface;
using Domain.Interface.Generic;
using Entities.Entities;
using Microsoft.AspNetCore.Mvc;
using Shared.Helper;
using System.Reflection;

namespace Domain.Services
{
    public class TaskHistoryService : ControllerBase, ITaskHistoryService
    {
        private readonly IGenericTaskHistoryInterface _genericTaskHistoryInterface;

        public TaskHistoryService(IGenericTaskHistoryInterface genericTaskHistoryInterface)
        {
            _genericTaskHistoryInterface = genericTaskHistoryInterface;
        }

        public async Task AddTaskHistoryByList(IEnumerable<ProjectTask> tasks)
        {
            foreach (var task in tasks)
            {
                await AddTaskHistory(task);
            }
        }

        public async Task AddTaskHistory(ProjectTask task, ProjectTask oldTask = default)
        {
            var taskHIstory = new ProjectTaskHistory
            {
                Status = task.Status,
                UpdateDate = DateTime.Now,
                UserId = task.UserId,
                TaskId = task.Id,
                History = GetChangedProperties(task, oldTask)
            };

            await _genericTaskHistoryInterface.Add(taskHIstory);
        }

        public async Task AddTaskCommentHistory(ProjectTask task, string comment)
        {
            var taskHIstory = new ProjectTaskHistory
            {
                Status = task.Status,
                UpdateDate = DateTime.Now,
                UserId = task.UserId,
                TaskId = task.Id,
                History = $"Comment added: {comment}"
            };

            await _genericTaskHistoryInterface.Add(taskHIstory);
        }

        private static string GetChangedProperties<T>(T current, T? original)
        {
            var changedProperties = new List<string>();
            var properties = typeof(T).GetProperties();

            foreach (var property in properties)
            {
                var rastrearAttribute = property.GetCustomAttribute<TrackLogAttribute>();
                if (rastrearAttribute != null)
                {
                    var currentValue = property.GetValue(current);
  
                    if (original is null)
                    {
                        changedProperties.Add($"{property.Name} atual: {currentValue}");
                        continue;
                    }

                    var originalValue = property.GetValue(original);

                    if (!Equals(originalValue, currentValue))
                    {
                        changedProperties.Add($"{property.Name} atual: {currentValue}, anterior: {originalValue}");
                    }
                }
            }

            return string.Join("; ", changedProperties);
        }
    }
}
