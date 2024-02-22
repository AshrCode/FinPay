using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseSchema;

namespace Persistence.User
{
    public class UserRepository : IUserRepository
    {
        // Another way of using repositories is to intorduce UnitOfWork pattern.
        protected readonly FinPayDbContext _dbContext;

        public UserRepository(FinPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Domain.Entities.User> Get(Guid id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        private List<KeyValuePair<Guid, Domain.Entities.User>> GetSampleUserData()
        {
            List<KeyValuePair<Guid, Domain.Entities.User>> items = new();

            // Sample User 1
            var key = new Guid("B136CF3D-766B-45AE-AA84-AC7F10C5A090");
            var value = new Domain.Entities.User
            {
                Id = key,
                IsVerified = true,
            };
            items.Add(new KeyValuePair<Guid, Domain.Entities.User>(key, value));

            // Sample User 2
            key = new Guid("6751304E-0EEA-443C-AD6A-DFBBF53731FE");
            value = new Domain.Entities.User
            {
                Id = key,
                IsVerified = false,
            };
            items.Add(new KeyValuePair<Guid, Domain.Entities.User>(key, value));

            return items;
        }
    }
}
