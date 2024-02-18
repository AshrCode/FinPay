using Application.Infrastructure;
using Common.ApiException;
using Domain.Entities;
using Domain.Enum;
using Microsoft.Extensions.Logging;
using Persistence.Beneficiary;
using Persistence.Transaction;
using Persistence.User;

namespace Application.Payment.Topup
{
    public class TopupApp : ITopupApp
    {
        private readonly IUserRepository _userRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBeneficiararyRepository _beneficiararyRepository;
        private readonly IBalanceService _balanceService;
        private readonly ILogger _logger;

        public TopupApp(ILogger<TopupApp> logger, IUserRepository userRepository, ITransactionRepository transactionRepository, IBeneficiararyRepository beneficiararyRepository, IBalanceService balanceService)
        {
            _logger = logger;
            _userRepository = userRepository;
            _transactionRepository = transactionRepository;
            _beneficiararyRepository = beneficiararyRepository;
            _balanceService = balanceService;
        }

        public async Task<Guid> MakePaymentAsync(Guid userId, Guid beneficiaryId, float amount)
        {
            // Validate user
            var user = await ValidateUser(userId);

            // Validate beneficiary
            await ValidateBeneficiary(beneficiaryId);

            // Get remaining top-up quota for the user
            var availableTopupQuota = await GetAvailableTopupQuota(user, beneficiaryId);

            // Raise exception if the top up amount is greater than availeble quota.
            if (amount > availableTopupQuota)
            {
                var errMessage = $"Top-up amount exceeded, available quota is {availableTopupQuota}.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.BadRequest, errMessage);
            }

            // Pay out
            float userBalance = await GetUserBalance(userId);
            var tranactionFee = 1F;
            var amountToPay = amount + tranactionFee;

            if (amountToPay > userBalance)
            {
                var errMessage = $"User {userId} doesn't have sufficient balance.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.BadRequest, errMessage);
            }

            // Deduct from the balance 
            await UpdateUserBalance(userId, amountToPay);

            // Save transaction
            var transactionId = Guid.NewGuid();

            Transaction trans = new()
            {
                Amount = amount,
                Fee = tranactionFee,
                BeneficiaryId = beneficiaryId,
                Id = transactionId,
                TransactionDate = DateTime.UtcNow,
                TransactionType = TransactionType.TopUp,
                UserId = userId
            };

            await _transactionRepository.Add(trans, transactionId);

            return transactionId;
        }

        private async Task<float> UpdateUserBalance(Guid userId, float amount)
        {
            var updatedBal = await _balanceService.DebitAmountAsync(userId, amount);
            
            return updatedBal;
        }

        private async Task<float> GetUserBalance(Guid userId)
        {
            var balance = await _balanceService.GetBalanceAsync(userId);
            return balance;
        }

        private async Task ValidateBeneficiary(Guid beneficiaryId)
        {
            var beneficiary = await _beneficiararyRepository.GetById(beneficiaryId);
            if (beneficiary is null)
            {
                var errMessage = $"Beneficiary with the ID {beneficiaryId} does not exist.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.NotFound, errMessage);
            }

            // Beneficiary exist but not active
            if (!beneficiary.IsActive)
            {
                var errMessage = $"Beneficiary with the ID {beneficiaryId} is not active.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.BadRequest, errMessage);
            }
        }

        private async Task<User> ValidateUser(Guid userId)
        {
            var user = await _userRepository.GetById(userId);
            if (user is null)
            {
                var errMessage = $"User with the ID {userId} does not exist.";
                _logger.LogWarning(errMessage);
                throw new ApiException(ApiErrorCodes.NotFound, errMessage);
            }

            return user;
        }

        /// <summary>
        /// Calculates the total available quota without fee.
        /// It depeneds upon the business to include or exclude the fee as a part.
        /// </summary>
        private async Task<float> GetAvailableTopupQuota(User user, Guid beneficiaryId)
        {
            // Ideally the following should be coming through a configuration table, which is out of the scope of the user story.
            float totalMonthlyLimit = 3000;
            float verifiedUserMonthlyLimit = 500; // as per the story
            float notVerifiedUserMonthlyLimit = 1000; // as per the story

            var monthTransactions = await _transactionRepository.GetForTheMonth(user.Id, TransactionType.TopUp);

            var totalMonthlyTrans = monthTransactions.Sum(x => x.Amount); // Excluding the fee

            // Check monthly limit
            if (totalMonthlyTrans >= totalMonthlyLimit)
                return 0;

            var remainingMonthlyTrans = totalMonthlyLimit - totalMonthlyTrans;

            // Sum of transactions to the specified beneficiary
            var beneficiaryTrans = monthTransactions.Where(x => x.BeneficiaryId == beneficiaryId).Sum(x => x.Amount); // For the total, excludin the fee.

            var remainingMonthlyBeneTrans = 0F;

            // For verified users
            if (user.IsVerified)
            {
                if (beneficiaryTrans >= verifiedUserMonthlyLimit)
                    return 0;
                else
                    remainingMonthlyBeneTrans = verifiedUserMonthlyLimit - beneficiaryTrans;
            }

            // For not verified users
            if (!user.IsVerified)
            {
                if (beneficiaryTrans >= notVerifiedUserMonthlyLimit)
                    return 0;
                else
                    remainingMonthlyBeneTrans = notVerifiedUserMonthlyLimit - beneficiaryTrans;
            }

            // Returns whichever is less
            return remainingMonthlyBeneTrans < remainingMonthlyTrans ? remainingMonthlyBeneTrans : remainingMonthlyTrans;
        }
    }
}
