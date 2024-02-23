using Application.Beneficiary;
using Common.ApiException;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using Persistence.Beneficiary;
using Persistence.User;

namespace Application.Test.Beneficiary
{
    [TestClass]
    public class BeneficiaryAppTest
    {
        private Mock<ILogger<BeneficiaryApp>> _mocklogger;
        private Mock<IUserRepository> _mockUserRepository;
        private Mock<IBeneficiararyRepository> _mockBeneficiararyRepository;
        private BeneficiaryApp _app;

        [TestInitialize]
        public void Setup()
        {
            _mocklogger = new Mock<ILogger<BeneficiaryApp>>();
            _mockUserRepository = new Mock<IUserRepository>();
            _mockBeneficiararyRepository = new Mock<IBeneficiararyRepository>();

            _app = new BeneficiaryApp(_mocklogger.Object, _mockBeneficiararyRepository.Object, _mockUserRepository.Object);
        }

        /// <summary>
        /// Should return the BeneficiaryId of type Guid when created successfully.
        /// </summary>
        [TestMethod]
        public async Task CreateAsync_ReturnsTheBeneficiaryIdIfCreatedSuccessfully()
        {
            // Arrange
            User user = new() { Id = Guid.NewGuid() };
            _mockUserRepository.Setup(a => a.Get(It.IsAny<Guid>()))
                                  .Returns(Task.FromResult(user));

            var beneficiaries = new List<Domain.Entities.Beneficiary>();
            _mockBeneficiararyRepository.Setup(b => b.GetAllBeneficiariesByUserId(user.Id)).Returns(Task.FromResult(beneficiaries));

            // Act
            var result = await _app.CreateAsync("Fam", user.Id, true);

            // Assert
            Assert.IsInstanceOfType<Guid>(result);
        }

        /// <summary>
        /// Should throw APIException when creating 6th active beneficiary.
        /// </summary>
        [TestMethod]
        public async Task CreateAsync_ThrowsApiExceptionWhenCreating6thActiveBeneficiary()
        {
            // Arrange
            User user = new() { Id = Guid.NewGuid() };
            _mockUserRepository.Setup(a => a.Get(It.IsAny<Guid>()))
                                  .Returns(Task.FromResult(user));

            var beneficiaries = new List<Domain.Entities.Beneficiary>()
            {
                new Domain.Entities.Beneficiary { Id = Guid.NewGuid(), IsActive = true },
                new Domain.Entities.Beneficiary { Id = Guid.NewGuid(), IsActive = true },
                new Domain.Entities.Beneficiary { Id = Guid.NewGuid(), IsActive = true },
                new Domain.Entities.Beneficiary { Id = Guid.NewGuid(), IsActive = true },
                new Domain.Entities.Beneficiary { Id = Guid.NewGuid(), IsActive = true },
            };
            _mockBeneficiararyRepository.Setup(b => b.GetAllBeneficiariesByUserId(user.Id)).Returns(Task.FromResult(beneficiaries));


            // Assert
            await Assert.ThrowsExceptionAsync<ApiException>(() => _app.CreateAsync("Sam", user.Id, true));
        }
    }
}
