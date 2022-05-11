using System.Linq;
using System.Threading;
using Features.Customers;
using MediatR;
using Moq;
using Moq.AutoMock;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(CustomerBogusCollection))]
    public class CustomerServiceAutoMockerTests
    {
        readonly CustomerTestsBogusFixture _customerTestsBogus;

        public CustomerServiceAutoMockerTests(CustomerTestsBogusFixture customerTestsFixture)
        {
            _customerTestsBogus = customerTestsFixture;
        }


        [Fact(DisplayName = "Add Customer with Successful")]
        [Trait("Category", "Customer Service AutoMock Tests")]
        public void CustomerService_Add_ShouldRunWithSuccess()        
        {
            // Arrange
            var customer = _customerTestsBogus.GenerateValidCustomer();
            var mocker = new AutoMocker();
            var customerService = mocker.CreateInstance<CustomerService>();

            // Act
            customerService.Add(customer);

            // Assert
            Assert.True(customer.IsValid());
            mocker.GetMock<ICustomerRepository>().Verify(r => r.Add(customer),Times.Once);
            mocker.GetMock<IMediator>().Verify(m=>m.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Add Customer with Error")]
        [Trait("Category", "Customer Service AutoMock Tests")]
        public void CustomerService_Add_ShouldRunWithInvalidCustomerError()        
        {
            // Arrange
            var customer = _customerTestsBogus.GenerateInvalidCustomer();
            var mocker = new AutoMocker();
            var customerService = mocker.CreateInstance<CustomerService>();

            // Act
            customerService.Add(customer);

            // Assert
            Assert.False(customer.IsValid());
            mocker.GetMock<ICustomerRepository>().Verify(r => r.Add(customer), Times.Never);
            mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Customers")]
        [Trait("Category", "Customer Service AutoMock Tests")]
        public void CustomerService_GetAllActives_ShouldRetunrOnlyActivesCustomers()        
        {
            // Arrange
            var mocker = new AutoMocker();
            var customerService = mocker.CreateInstance<CustomerService>();

            mocker.GetMock<ICustomerRepository>().Setup(c => c.GetAll())
                .Returns(_customerTestsBogus.GetRandomCustomers());

            // Act
            var clientes = customerService.GetAllActives();

            // Assert 
            mocker.GetMock<ICustomerRepository>().Verify(r => r.GetAll(), Times.Once);
            Assert.True(clientes.Any());
            Assert.False(clientes.Count(c=>!c.Active) > 0);
        }
    }
}