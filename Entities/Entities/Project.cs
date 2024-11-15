using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace Entities.Entities
{
    public class Project : BaseEntity
    {
        [StringLength(30)]
        public string Name { get; set; } = default!;

        [ForeignKey("Users")]
        public Guid UserId { get; set; }

        [JsonIgnore]
        public ProjectUser User { get; set; }

        public DateTime CreateDate { get; set; } = DateTime.Now;

        public IEnumerable<ProjectTask>? Tasks { get; set; }
    }
}
