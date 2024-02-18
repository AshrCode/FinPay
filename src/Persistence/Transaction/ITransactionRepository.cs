using Domain.Enum;

namespace Persistence.Transaction
{
    public interface ITransactionRepository
    {
        Task<List<Domain.Entities.Transaction>> GetForTheMonth(Guid userId, TransactionType transactionType);

        Task Add(Domain.Entities.Transaction transaction, Guid key);
    }
}
