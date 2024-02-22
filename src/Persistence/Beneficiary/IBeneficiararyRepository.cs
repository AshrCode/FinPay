namespace Persistence.Beneficiary
{
    public interface IBeneficiararyRepository
    {
        Task<Domain.Entities.Beneficiary> Get(Guid id, Guid userId);

        Task<List<Domain.Entities.Beneficiary>> GetAllBeneficiariesByUserId(Guid userId);

        Task Save(Domain.Entities.Beneficiary beneficiary);

    }
}
