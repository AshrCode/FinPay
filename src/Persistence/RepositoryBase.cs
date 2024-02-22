using Common.ApiException;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Persistence
{
    [Obsolete]
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected static IDictionary<Guid, T> _storage = new ConcurrentDictionary<Guid, T>();
        protected readonly ILogger<RepositoryBase<T>> _logger;

        public RepositoryBase(ILogger<RepositoryBase<T>> logger)
        {
            _logger = logger;
        }

        public Task<T> Get(Guid key)
        {
            _storage.TryGetValue(key, out T val);

            return Task.FromResult(val);
        }

        public async Task Save(Guid key, T value)
        {
            if (value is null) 
            {
                var errMessage = $"Unable to save. Value is null";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.BadRequest, errMessage);
            }
            
            var isSuccess = _storage.TryAdd(key, value);

            if (!isSuccess)
            {
                var errMessage = $"Unable to save. Key {key} already exist.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.BadRequest, errMessage);
            }
        }
    }
}
