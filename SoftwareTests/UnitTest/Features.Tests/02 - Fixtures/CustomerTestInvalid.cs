using Xunit;

namespace Features.Tests
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerTestInvalid
    {
        private readonly CustomerTestsFixture _customerTestsFixture;

        public CustomerTestInvalid(CustomerTestsFixture customerTestsFixture)
        {
            _customerTestsFixture = customerTestsFixture;
        }

        [Fact(DisplayName = "New Customer Invalid")]
        [Trait("Category", "Customer Fixture Tests")]
        public void Customer_NewCustomer_ShouldBeInvalid()
        {
            // Arrange
            var customer = _customerTestsFixture.GenerateCustomerInvalid();

            // Act
            var result = customer.IsValid();

            // Assert 
            Assert.False(result);
            Assert.NotEqual(0, customer.ValidationResult.Errors.Count);
        }
    }
}