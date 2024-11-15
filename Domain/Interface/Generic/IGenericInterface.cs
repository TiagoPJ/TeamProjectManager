namespace Domain.Interface.Generic
{
    public interface IGenericInterface<T> where T : class
    {
        Task Add(T Object);
        Task Update(T Object);
        Task Delete(T Object);
        Task<T> GetEntityById(Guid Id);
        Task<List<T>> List();
    }
}
