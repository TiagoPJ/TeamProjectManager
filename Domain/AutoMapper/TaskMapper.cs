using Entities.Entities;
using Shared.DTO;

namespace Domain.AutoMapper
{
    public partial class EntityToDto
    {
        private void MapTask()
        {
            CreateMap<TaskDto, ProjectTask>()
                .ForMember(x => x.UserId, x => x.MapFrom(s => s.UserId))
                .ForMember(x => x.Status, x => x.MapFrom(s => s.Status))
                .ForMember(x => x.Description, x => x.MapFrom(s => s.Description))
                .ForMember(x => x.Priority, x => x.MapFrom(s => s.Priority))
                .ForMember(x => x.Title, x => x.MapFrom(s => s.Title))
                .ForMember(x => x.ExpirationDate, x => x.MapFrom(s => s.ExpirationDate))
                .ForMember(dest => dest.Comments, opt => opt.MapFrom(s => ResolveIdUserTask(s)))
                .ReverseMap();
        }

        private static IEnumerable<TaskCommentsDto>? ResolveIdUserTask(TaskDto task)
        {
            if (task.Comments is null)
                return default;

            foreach (var comment in task.Comments)
            {
                comment.UserId = task.UserId;
            }

            return task.Comments;
        }
    }
}
