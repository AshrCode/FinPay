namespace Persistence
{
    public interface IRepositoryBase<T>
    {
        Task<T> Get(Guid key);
    }
}
