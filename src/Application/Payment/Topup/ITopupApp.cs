namespace Application.Payment.Topup
{
    public interface ITopupApp
    {
        Task MakePayment(int userId, int beneficiaryId, int amount);
    }
}
