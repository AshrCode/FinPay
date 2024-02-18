namespace Application.Infrastructure
{
    public interface IBalanceService
    {
        Task<float> GetBalanceAsync(Guid userId);

        Task<float> DebitAmountAsync(Guid userId, float amountToDebit);
    }
}
