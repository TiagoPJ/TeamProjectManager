using Shared.Enums;
using Shared.Helper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Entities
{
    public class ProjectTask : BaseEntity, ICloneable
    {
        [ForeignKey("Projects")]
        public Guid ProjectId { get; set; }

        [ForeignKey("Users")]
        [TrackLog]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public ProjectUser User { get; set; }

        [StringLength(50)]
        [TrackLog]
        public string Title { get; set; } = string.Empty;

        [StringLength(150)]
        [TrackLog]
        public string Description { get; set; } = string.Empty;
        [TrackLog]
        public DateTime ExpirationDate { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [TrackLog]
        public Status Status { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        [TrackLog]
        public Priority Priority { get; set; }

        public IEnumerable<ProjectTaskComments>? Comments { get; set; }

        public object Clone()
        {
            ProjectTask task = (ProjectTask)this.MemberwiseClone();
            return task;
        }
    }
}
