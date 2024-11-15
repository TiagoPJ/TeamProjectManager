using Entities.Entities;
using Shared.DTO;

namespace Domain.AutoMapper
{
    public partial class EntityToDto
    {
        private void MapTaskComment()
        {
            CreateMap<TaskCommentsDto, ProjectTaskComments>()
                .ForMember(x => x.UserId, x => x.MapFrom(s => s.UserId))
                .ForMember(x => x.TaskId, x => x.MapFrom(s => s.TaskId))
                .ForMember(x => x.Comment, x => x.MapFrom(s => s.Comment))
                .ReverseMap();
        }
    }
}
