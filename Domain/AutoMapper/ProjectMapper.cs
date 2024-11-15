using Entities.Entities;
using Shared.DTO;

namespace Domain.AutoMapper
{
    public partial class EntityToDto
    {
        private void MapProject()
        {
            CreateMap<ProjectDto, Project>()
                .ForMember(x => x.UserId, x => x.MapFrom(s => s.UserId))
                .ForMember(x => x.Name, x => x.MapFrom(s => s.Name))
                .ForMember(dest => dest.Tasks, opt => opt.MapFrom(s => ResolveIdUserTask(s)))
                .ReverseMap();

        }

        private static IEnumerable<TaskDto>? ResolveIdUserTask(ProjectDto project)
        {
            if (project.Tasks is null)
                return default;

            foreach (var task in project.Tasks)
            {
                task.UserId = project.UserId;
            }

            return project.Tasks;
        }
    }
}
