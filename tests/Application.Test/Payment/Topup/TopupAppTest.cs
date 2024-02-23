using Application.Infrastructure;
using Application.Payment.Topup;
using Common.ApiException;
using Domain.Entities;
using Domain.Enum;
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

        /// <summary>
        /// Should throw ApiException when invalid BeneficiaryId is provided.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ThrowsApiExceptionWhenInValidBeneficiaryIdIsProvided()
        {
            // Arrange
            User user = new();
            _mockUserRepository.Setup(a => a.Get(It.IsAny<Guid>()))
                                  .Returns(Task.FromResult(user));

            var validUserId = Guid.NewGuid();
            var invalidBeneficiaryId = Guid.NewGuid();

            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.MakePaymentAsync(validUserId, invalidBeneficiaryId, 50));
        }

        /// <summary>
        /// Should throw ApiException when beneficiary is not active.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ThrowsApiExceptionWhenBeneficiaryIsNotActive()
        {
            // Arrange
            var beneficiary = InitiateMockBeneficiaryAndUser(true, false);

            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.MakePaymentAsync(beneficiary.UserId, beneficiary.Id, 50));
        }

        /// <summary>
        /// Should throw ApiException when the quota for verified user exceeds 500.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ThrowsApiExceptionWhenQuotaForVerifiedUserExceeds500()
        {
            // Arrange
            var beneficiary = InitiateMockBeneficiaryAndUser(true, true);

            List<Transaction> transactions = new List<Transaction>();
            _mockTransactionRepository.Setup(t => t.GetForTheMonth(It.IsAny<Guid>(), TransactionType.TopUp)).Returns(Task.FromResult(transactions));

            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.MakePaymentAsync(beneficiary.UserId, beneficiary.Id, 600));
        }

        /// <summary>
        /// Should throw ApiException when the quota for unverified user exceeds 1000.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ThrowsApiExceptionWhenQuotaForUnverifiedUserExceeds1000()
        {
            // Arrange
            var beneficiary = InitiateMockBeneficiaryAndUser(false, true);

            List<Transaction> transactions = new List<Transaction>();
            _mockTransactionRepository.Setup(t => t.GetForTheMonth(It.IsAny<Guid>(), TransactionType.TopUp)).Returns(Task.FromResult(transactions));

            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.MakePaymentAsync(beneficiary.UserId, beneficiary.Id, 1100));
        }

        /// <summary>
        /// Should throw ApiException when the amount-to-pay is more than the actual balance.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ThrowsApiExceptionWhenAmountToPayIsMoreThanBalance()
        {
            // Arrange
            var beneficiary = InitiateMockBeneficiaryAndUser(false, true);

            List<Transaction> transactions = new List<Transaction>();
            _mockTransactionRepository.Setup(t => t.GetForTheMonth(It.IsAny<Guid>(), TransactionType.TopUp)).Returns(Task.FromResult(transactions));

            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.MakePaymentAsync(beneficiary.UserId, beneficiary.Id, 500));
        }

        /// <summary>
        /// Should return the TransactionId of type Guid when the payment is successfully.
        /// </summary>
        [TestMethod]
        public async Task MakePaymentAsync_ReturnsTheTransactionIdWhenPaymentIsSuccessful()
        {
            // Arrange
            var beneficiary = InitiateMockBeneficiaryAndUser(false, true);

            List<Transaction> transactions = new List<Transaction>();
            _mockTransactionRepository.Setup(t => t.GetForTheMonth(It.IsAny<Guid>(), TransactionType.TopUp)).Returns(Task.FromResult(transactions));

            _mockBalanceService.Setup(b => b.GetBalanceAsync(It.IsAny<Guid>())).Returns(Task.FromResult(900F));

            // Act
            var result = await _app.MakePaymentAsync(beneficiary.UserId, beneficiary.Id, 500);

            // Assert
            Assert.IsInstanceOfType<Guid>(result);
        }

        /// <summary>
        /// Initistes common mock objects for User and beneficiary entities.
        /// </summary>
        /// <param name="isUserVerified">Represents if the user is verified</param>
        /// <param name="isBeneficiaryActive">Represents if the beneficiary is active</param>
        /// <returns>Instance of Beneficiary class</returns>
        private Domain.Entities.Beneficiary InitiateMockBeneficiaryAndUser(bool isUserVerified, bool isBeneficiaryActive)
        {
            User user = new() { IsVerified = isUserVerified };
            _mockUserRepository.Setup(a => a.Get(It.IsAny<Guid>()))
                                  .Returns(Task.FromResult(user));

            Domain.Entities.Beneficiary beneficiary = new() { Id = Guid.NewGuid(), UserId = Guid.NewGuid(), IsActive = isBeneficiaryActive };
            _mockBeneficiararyRepository.Setup(a => a.Get(It.IsAny<Guid>(), It.IsAny<Guid>()))
                                  .Returns(Task.FromResult(beneficiary));
            return beneficiary;
        }
    }
}
