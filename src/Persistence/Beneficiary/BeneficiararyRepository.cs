using Microsoft.Extensions.Logging;

namespace Persistence.Beneficiary
{
    public class BeneficiararyRepository : RepositoryBase<Domain.Entities.Beneficiary>, IBeneficiararyRepository
    {
        public BeneficiararyRepository(ILogger<BeneficiararyRepository> logger) 
            : base(logger)
        {
        }

        public async Task Create(Guid beneficiaryId, Domain.Entities.Beneficiary beneficiary)
        {
            await Save(beneficiaryId, beneficiary);
        }

        public Task<List<Domain.Entities.Beneficiary>> GetAllBeneficiariesByUserId(Guid userId)
        {
            var beneficiaries = _storage.Where(s => s.Value.UserId == userId)
                                        .Select(s => s.Value).ToList(); 
            
            return Task.FromResult(beneficiaries);
        }

        public async Task<Domain.Entities.Beneficiary> GetById(Guid id)
        {
            return await Get(id);
        }
    }
}
