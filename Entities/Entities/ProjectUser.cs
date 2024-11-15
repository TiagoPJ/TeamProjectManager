using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entities
{
    public class ProjectUser : BaseEntity
    {
        [StringLength(30)]
        public string Name { get; set; } = string.Empty;
        public Position Position { get; set; } = Position.Simple;
    }
}
