namespace Shared.DTO
{
    public class TaskCommentsDto
    {
        public Guid TaskId { get; set; }
        public Guid UserId { get; set; }
        public string Comment { get; set; }
    }
}
