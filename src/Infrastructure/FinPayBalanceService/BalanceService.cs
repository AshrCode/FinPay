using Application.Infrastructure;
using Common.ApiException;
using Common.Configuration;
using Infrastructure.Models;
using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Infrastructure.FinPayBalanceService
{
    public class BalanceService : IBalanceService
    {
        private readonly ILogger<BalanceService> _logger;
        private readonly FinPayBalanceServiceSettings _settings;
        private readonly HttpClient _httpClient;

        public BalanceService(ILogger<BalanceService> logger, FinPayBalanceServiceSettings finPayBalanceServiceSettings)
        {
            _logger = logger;
            _settings = finPayBalanceServiceSettings;
            _httpClient = new HttpClient();
        }

        public async Task<float> DebitAmountAsync(Guid userId, float amountToDebit)
        {
            UserAccountModel userAccount = null;
            var content = new StringContent(amountToDebit.ToString());
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            HttpResponseMessage response = await _httpClient.PutAsync(_settings.BaseUrl + $"api/AccountBalance/DebitAmount/{userId}", content);
            if (!response.IsSuccessStatusCode)
            {
                var errMessage = $"Failed to debit the amount {amountToDebit} from the account. User Id: {userId}, StatusCode: {response.StatusCode}.";
                _logger.LogError(errMessage);
                throw new ApiException(ApiErrorCodes.InternalError, errMessage);
            }

            userAccount = await response.Content.ReadFromJsonAsync<UserAccountModel>();

            return userAccount.Data.Balance;
        }

        public async Task<float> GetBalanceAsync(Guid userId)
        {
            UserAccountModel userAccount = null;
            HttpResponseMessage response = await _httpClient.GetAsync(_settings.BaseUrl + $"api/AccountBalance/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                var errMessage = $"Failed to get account imfofmetion. User Id: {userId}, StatusCode: {response.StatusCode}.";
                _logger.LogError(errMessage);
                throw new ApiException(ApiErrorCodes.InternalError, errMessage);
            }

            userAccount = await response.Content.ReadFromJsonAsync<UserAccountModel>();

            return userAccount.Data.Balance;
        }
    }
}
