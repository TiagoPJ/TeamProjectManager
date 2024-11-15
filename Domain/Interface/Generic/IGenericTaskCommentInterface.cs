using Entities.Entities;

namespace Domain.Interface.Generic
{
    public interface IGenericTaskCommentInterface : IGenericInterface<ProjectTaskComments>
    {
        Task<IEnumerable<ProjectTaskComments>> GetCommentsByUser(Guid userId);
    }
}
