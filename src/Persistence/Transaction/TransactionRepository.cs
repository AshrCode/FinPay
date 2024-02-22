using Domain.Enum;
using Persistence.DatabaseSchema;

namespace Persistence.Transaction
{
    public class TransactionRepository : ITransactionRepository
    {
        // Another way of using repositories is to intorduce UnitOfWork pattern.
        protected readonly FinPayDbContext _dbContext;

        public TransactionRepository(FinPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Add(Domain.Entities.Transaction transaction)
        {
            await _dbContext.AddAsync(transaction);
            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Domain.Entities.Transaction>> GetForTheMonth(Guid userId, TransactionType transactionType)
        {
            var data = _dbContext.Transactions.Where(t => t.UserId == userId 
                                                    && t.TransactionType == transactionType
                                                    && t.TransactionDate.Month == DateTime.Now.Month).ToList();

            return Task.FromResult(data);
        }
    }
}
