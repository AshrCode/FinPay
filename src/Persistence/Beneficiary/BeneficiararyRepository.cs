using Microsoft.EntityFrameworkCore;
using Persistence.DatabaseSchema;

namespace Persistence.Beneficiary
{
    public class BeneficiararyRepository : IBeneficiararyRepository
    {
        // Another way of using repositories is to intorduce UnitOfWork pattern.
        protected readonly FinPayDbContext _dbContext;

        public BeneficiararyRepository(FinPayDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Save(Domain.Entities.Beneficiary beneficiary)
        {
            var benef = await Get(beneficiary.Id, beneficiary.UserId);
            if (benef is null)
            {
                await _dbContext.AddAsync(beneficiary);
            }
            else
            {
                benef.IsActive = beneficiary.IsActive;
                benef.NickName = beneficiary.NickName; // As per common banking functionality a user can have multiple beneficiaries with a common nickname but with different bank account numbers.
            }

            await _dbContext.SaveChangesAsync();
        }

        public Task<List<Domain.Entities.Beneficiary>> GetAllBeneficiariesByUserId(Guid userId)
        {
            var beneficiaries = _dbContext.Beneficiaries.Where(s => s.UserId == userId)?.ToList();

            return Task.FromResult(beneficiaries);
        }

        public async Task<Domain.Entities.Beneficiary> Get(Guid id, Guid userId)
        {
            return await _dbContext.Beneficiaries.FirstOrDefaultAsync(x =>x.Id == id && x.UserId == userId);
        }
    }
}
