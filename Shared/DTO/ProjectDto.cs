namespace Shared.DTO
{
    public class ProjectDto
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public IEnumerable<TaskDto>? Tasks { get; set; }
    }
}
