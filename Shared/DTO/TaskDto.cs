using Shared.Enums;
using System.Text.Json.Serialization;

namespace Shared.DTO
{
    public class TaskDto
    {
        public Guid ProjectId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime ExpirationDate { get; set; }
        public Status Status { get; set; }
        public Priority Priority { get; set; }
        public IEnumerable<TaskCommentsDto>? Comments { get; set; }
    }
}
