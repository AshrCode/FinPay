using Microsoft.Extensions.Logging;

namespace Persistence.User
{
    public class UserRepository : RepositoryBase<Domain.Entities.User>, IUserRepository
    {
        public UserRepository(ILogger<UserRepository> logger) 
            : base(logger)
        {
            if (_storage.Count < 1)
            {
                // Seeding sample user data.
                List<KeyValuePair<Guid, Domain.Entities.User>> items = GetSampleUserData();
                items.ForEach(x => _storage.Add(x.Key, x.Value));

                _logger.LogInformation("Sample decision tree data has been seeded.");
            }
        }

        public async Task<Domain.Entities.User> GetById(Guid id)
        {
            /*
             * Here, we can add additional repository level functionality in future if needed.
             */

            return await Get(id);
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
