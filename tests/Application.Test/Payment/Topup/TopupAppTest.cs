using Application.Infrastructure;
using Application.Payment.Topup;
using Common.ApiException;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Beneficiary;
using Persistence.Transaction;
using Persistence.User;

namespace Application.Test.Payment.Topup
{
    [TestClass]
    public class TopupAppTest
    {
        private Mock<ILogger<TopupApp>> _mocklogger;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<ITransactionRepository> _mockTransactionRepository;
        private Mock<IBeneficiararyRepository> _mockBeneficiararyRepository;
        private Mock<IBalanceService> _mockBalanceService;
        private TopupApp _app;

        [TestInitialize]
        public void Setup()
        {
            _mocklogger = new Mock<ILogger<TopupApp>>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockTransactionRepository = new Mock<ITransactionRepository>();
            _mockBeneficiararyRepository = new Mock<IBeneficiararyRepository>();
            _mockBalanceService = new Mock<IBalanceService>();

            _app = new TopupApp(_mocklogger.Object, _mockUserRepository.Object, _mockTransactionRepository.Object, _mockBeneficiararyRepository.Object, _mockBalanceService.Object);
        }

        /// <summary>
        /// Should throw ApiException when invalid UserId is provided.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ThrowsApiExceptionWhenInValidUserIdIsProvided()
        {
            // Arrange
            var invalidUserId = Guid.NewGuid();
            var beneficiaryId = Guid.NewGuid();

            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.MakePaymentAsync(invalidUserId, beneficiaryId, 50));
        }
    }
}
