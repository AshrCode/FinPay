namespace Application.Payment.Topup
{
    public interface ITopupApp
    {
        Task<Guid> MakePaymentAsync(Guid userId, Guid beneficiaryId, float amount);
    }
}
