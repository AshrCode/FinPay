using Common.ApiException;
using Microsoft.Extensions.Logging;
using Persistence.Beneficiary;
using Persistence.User;

namespace Application.Beneficiary
{
    public class BeneficiaryApp : IBeneficiaryApp
    {
        private readonly IBeneficiararyRepository _beneficiararyRepository;
        private readonly IUserRepository _userRepository;

        private readonly ILogger<BeneficiaryApp> _logger;

        public BeneficiaryApp(ILogger<BeneficiaryApp> logger, IBeneficiararyRepository beneficiararyRepository, IUserRepository userRepository)
        {
            _beneficiararyRepository = beneficiararyRepository;
            _userRepository = userRepository;
            _logger = logger;
        }


        public async Task<Guid> CreateAsync(string nickName, Guid userId, bool isActive)
        {
            // Check for 5 active beneficiaries
            var ActiveBeneficiaries = await GetAllAsync(userId, isActive);
            if (ActiveBeneficiaries.Count >= 5)
            {
                var errMessage = $"User {userId} already have 5 active beneficiaries.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.BadRequest, errMessage);
            }

            var beneficiaryId = Guid.NewGuid();
            Domain.Entities.Beneficiary beneficiary = new()
            {
                Id = beneficiaryId,
                IsActive = isActive,
                NickName = nickName,
                UserId = userId,
            };

            await _beneficiararyRepository.Save(beneficiary);

            return beneficiaryId;
        }

        public async Task<List<Domain.Entities.Beneficiary>> GetAllAsync(Guid userId, bool isActive)
        {
            // Validate user
            await ValidateUser(userId);

            var activeBeneficiaries = await _beneficiararyRepository.GetAllBeneficiariesByUserId(userId);

            // Return only active ones
            return activeBeneficiaries.Where(x => x.IsActive == isActive).ToList();
                                                
        }

        private async Task ValidateUser(Guid userId)
        {
            var user = await _userRepository.Get(userId);
            if (user is null)
            {
                var errMessage = $"User with the ID {userId} does not exist.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.NotFound, errMessage);
            }
        }
    }
}
