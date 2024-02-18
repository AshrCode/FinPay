using Domain.Enum;
using Microsoft.Extensions.Logging;

namespace Persistence.Transaction
{
    public class TransactionRepository : RepositoryBase<Domain.Entities.Transaction>, ITransactionRepository
    {
        public TransactionRepository(ILogger<TransactionRepository> logger) 
            : base(logger)
        {
        }

        public async Task Add(Domain.Entities.Transaction transaction, Guid key)
        {
            await Save(key, transaction);
        }

        public Task<List<Domain.Entities.Transaction>> GetForTheMonth(Guid userId, TransactionType transactionType)
        {
            var data = _storage.Where(x => x.Value.UserId == userId 
                                && x.Value.TransactionType == transactionType
                                && x.Value.TransactionDate.Month == DateTime.Now.Month)
                               .Select(s => s.Value).ToList();

            return Task.FromResult(data);
        }


    }
}
