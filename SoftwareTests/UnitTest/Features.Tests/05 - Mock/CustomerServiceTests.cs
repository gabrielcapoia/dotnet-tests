using System.Linq;
using System.Threading;
using Features.Customers;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(CustomerBogusCollection))]
    public class CustomerServiceTests
    {
        readonly CustomerTestsBogusFixture _customerTestsBogus;

        public CustomerServiceTests(CustomerTestsBogusFixture customerTestsFixture)
        {
            _customerTestsBogus = customerTestsFixture;
        }

        [Fact(DisplayName = "Add Customer with Successful")]
        [Trait("Category", "Customer Service Mock Tests")]
        public void CustomerService_Add_ShouldRunWithSuccess()
        {
            // Arrange
            var customer = _customerTestsBogus.GenerateValidCustomer();
            var customerRepo = new Mock<ICustomerRepository>();
            var mediatr = new Mock<IMediator>();

            var customerService = new CustomerService(customerRepo.Object, mediatr.Object);

            // Act
            customerService.Add(customer);

            // Assert
            Assert.True(customer.IsValid());
            customerRepo.Verify(r => r.Add(customer),Times.Once);
            mediatr.Verify(m=>m.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Add Customer with Error")]
        [Trait("Category", "Customer Service Mock Tests")]
        public void CustomerService_Add_ShouldRunWithInvalidCustomerError()
        {
            // Arrange
            var customer = _customerTestsBogus.GenerateInvalidCustomer();
            var customerRepo = new Mock<ICustomerRepository>();
            var mediatr = new Mock<IMediator>();

            var customerService = new CustomerService(customerRepo.Object, mediatr.Object);

            // Act
            customerService.Add(customer);

            // Assert
            Assert.False(customer.IsValid());
            customerRepo.Verify(r => r.Add(customer), Times.Never);
            mediatr.Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Customers")]
        [Trait("Category", "Customer Service Mock Tests")]
        public void CustomerService_GetAllActives_ShouldRetunrOnlyActivesCustomers()
        {
            // Arrange
            var customerRepo = new Mock<ICustomerRepository>();
            var mediatr = new Mock<IMediator>();

            customerRepo.Setup(c => c.GetAll())
                .Returns(_customerTestsBogus.GetRandomCustomers());

            var customerService = new CustomerService(customerRepo.Object, mediatr.Object);

            // Act
            var customers = customerService.GetAllActives();

            // Assert 
            customerRepo.Verify(r => r.GetAll(), Times.Once);
            Assert.True(customers.Any());
            Assert.False(customers.Count(c=>!c.Active) > 0);
        }
    }
}