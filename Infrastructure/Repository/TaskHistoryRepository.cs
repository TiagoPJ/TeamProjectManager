using Domain.Interface.Generic;
using Entities.Entities;
using Infrastructure.Repository.Generic;

namespace Infrastructure.Repository
{
    public class TaskHistoryRepository : GenericRepository<ProjectTaskHistory>, IGenericTaskHistoryInterface
    {
    }
}
