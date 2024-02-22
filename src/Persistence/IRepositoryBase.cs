namespace Persistence
{
    [Obsolete]
    public interface IRepositoryBase<T>
    {
        Task<T> Get(Guid key);
    }
}
