namespace Application.Beneficiary
{
    public interface IBeneficiaryApp
    {
        Task<Guid> CreateAsync(string nickName, Guid userId, bool isActive);

        Task<List<Domain.Entities.Beneficiary>> GetAllAsync(Guid userId, bool isActive);
    }
}
