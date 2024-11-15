using AutoMapper;

namespace Domain.AutoMapper
{
    public partial class EntityToDto : Profile
    {
        public EntityToDto()
        {
            MapProject();
            MapTask();
            MapTaskComment();
        }
    }
}
