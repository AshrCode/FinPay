namespace Persistence.User
{
    public interface IUserRepository
    {
        Task<Domain.Entities.User> GetById(Guid id);
    }
}
