using Shared.Enums;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Entities
{
    public class ProjectTaskHistory : BaseEntity
    {
        public ProjectTaskHistory() { }

        [ForeignKey("Tasks")]
        public Guid TaskId { get; set; }

        [JsonIgnore]
        public ProjectTask Task { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public ProjectUser User { get; set; }

        public DateTime UpdateDate { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]

        public DateTime CreateDate { get; set; } = DateTime.Now;
        public Status Status { get; set; }
        public string History { get; set; }
    }
}
