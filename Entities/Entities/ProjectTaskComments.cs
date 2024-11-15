using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Entities
{
    public class ProjectTaskComments : BaseEntity
    {
        [ForeignKey("Tasks")]
        public Guid TaskId { get; set; }

        [JsonIgnore]
        public ProjectTask Task { get; set; }

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public ProjectUser User { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        [StringLength(200)]
        public string Comment { get; set; }
    }
}
