namespace Persistence.Beneficiary
{
    public interface IBeneficiararyRepository
    {
        Task<Domain.Entities.Beneficiary> GetById(Guid id);

        Task<List<Domain.Entities.Beneficiary>> GetAllBeneficiariesByUserId(Guid userId);

        Task Create(Guid beneficiaryId, Domain.Entities.Beneficiary beneficiary);

    }
}
