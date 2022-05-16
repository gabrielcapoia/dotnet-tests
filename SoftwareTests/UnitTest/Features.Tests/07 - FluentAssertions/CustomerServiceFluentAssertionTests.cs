using System.Linq;
using System.Threading;
using Features.Customers;
using FluentAssertions;
using FluentAssertions.Extensions;
using MediatR;
using Moq;
using Xunit;

namespace Features.Tests
{
    [Collection(nameof(CustomerAutoMockerCollection))]
    public class CustomerServiceFluentAssertionTests
    {
        readonly CustomerTestsAutoMockerFixture _customerTestsAutoMockerFixture;

        private readonly CustomerService _customerService;

        public CustomerServiceFluentAssertionTests(CustomerTestsAutoMockerFixture customerTestsFixture)
        {
            _customerTestsAutoMockerFixture = customerTestsFixture;
            _customerService = _customerTestsAutoMockerFixture.GetCustomerService();
        }

        [Fact(DisplayName = "Add Customer with Successful")]
        [Trait("Category", "Customer Service Fluent Assertion Tests")]
        public void CustomerService_Add_ShouldRunWithSuccess()        
        {
            // Arrange
            var customer = _customerTestsAutoMockerFixture.GenerateValidCustomer();

            // Act
            _customerService.Add(customer);

            // Assert
            //Assert.True(customer.EhValido());

            // Assert
            customer.IsValid().Should().BeTrue();

            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Verify(r => r.Add(customer),Times.Once);
            _customerTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m=>m.Publish(It.IsAny<INotification>(),CancellationToken.None),Times.Once);
        }

        [Fact(DisplayName = "Add Customer with Error")]
        [Trait("Category", "Customer Service Fluent Assertion Tests")]
        public void CustomerService_Add_ShouldRunWithInvalidCustomerError()        
        {
            // Arrange
            var customer = _customerTestsAutoMockerFixture.GenerateInvalidCustomer();

            // Act
            _customerService.Add(customer);

            // Assert
            Assert.False(customer.IsValid());

            // Assert
            customer.IsValid().Should().BeFalse("There are inconsistencies");
            customer.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1);

            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Verify(r => r.Add(customer), Times.Never);
            _customerTestsAutoMockerFixture.Mocker.GetMock<IMediator>().Verify(m => m.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Never);
        }

        [Fact(DisplayName = "Get Actives Customers")]
        [Trait("Category", "Customer Service Fluent Assertion Tests")]
        public void CustomerService_GetAllActives_ShouldRetunrOnlyActivesCustomers()        
        {
            // Arrange
            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Setup(c => c.GetAll())
                .Returns(_customerTestsAutoMockerFixture.GetRandomCustomers());

            // Act
            var customers = _customerService.GetAllActives();

            // Assert 
            //Assert.True(customers.Any());
            //Assert.False(customers.Count(c => !c.Ativo) > 0);

            // Assert
            customers.Should().HaveCountGreaterOrEqualTo(1).And.OnlyHaveUniqueItems();
            customers.Should().NotContain(c => !c.Active);

            _customerTestsAutoMockerFixture.Mocker.GetMock<ICustomerRepository>().Verify(r => r.GetAll(), Times.Once);

            _customerService.ExecutionTimeOf(c=>c.GetAllActives())
                .Should()
                .BeLessOrEqualTo(50.Milliseconds(),
                    "runs thousands of times per second");
        }
    }
}