using System.Linq;
using System.Threading;
using Features.Customers;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(CustomerAutoMockerCollection))]
    public class CustomerServiceAutoMockerFixtureTests
    {
        readonly CustomerTestsAutoMockerFixture _customerTestsAutoMockerFixture;

        private readonly CustomerService _customerService;

        public CustomerServiceAutoMockerFixtureTests(CustomerTestsAutoMockerFixture customerTestsFixture)
        {
            _customerTestsAutoMockerFixture = customerTestsFixture;
            _customerService = _customerTestsAutoMockerFixture.GetCustomerService();
        }

        [Fact(DisplayName = "Add Customer with Successful")]
        [Trait("Category", "Customer Service AutoMockFixture Tests")]
        public void CustomerService_Add_ShouldRunWithSuccess()
        {
            // Arrange
            var customer = _customerTestsAutoMockerFixture.GenerateValidCustomer();

            // Act
            _customerService.Add(customer);

            // Assert
            Assert.True(customer.IsValid());
            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Verify(r => r.Add(customer),Times.Once);
            _customerTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m=>m.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Add Customer with Error")]
        [Trait("Category", "Customer Service AutoMockFixture Tests")]
        public void CustomerService_Add_ShouldRunWithInvalidCustomerError()
        
        {
            // Arrange
            var customer = _customerTestsAutoMockerFixture.GenerateInvalidCustomer();

            // Act
            _customerService.Add(customer);

            // Assert
            Assert.False(customer.IsValid());
            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Verify(r => r.Add(customer), Times.Never);
            _customerTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Customers")]
        [Trait("Category", "Customer Service AutoMockFixture Tests")]
        public void CustomerService_GetAllActives_ShouldRetunrOnlyActivesCustomers()
        {
            // Arrange
            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Setup(c => c.GetAll())
                .Returns(_customerTestsAutoMockerFixture.GetRandomCustomers());

            // Act
            var customer = _customerService.GetAllActives();

            // Assert 
            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Verify(r => r.GetAll(), Times.Once);
            Assert.True(customer.Any());
            Assert.False(customer.Count(c=>!c.Active) > 0);
        }
    }
}