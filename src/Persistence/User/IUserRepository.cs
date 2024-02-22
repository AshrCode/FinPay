namespace Persistence.User
{
    public interface IUserRepository
    {
        Task<Domain.Entities.User> Get(Guid id);
    }
}
