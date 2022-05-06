using Xunit;

namespace Features.Tests
{
    [Collection(nameof(CustomerCollection))]
    public class CustomerValidTest
    {
        private readonly CustomerTestsFixture _customerTestsFixture;

        public CustomerValidTest(CustomerTestsFixture customerTestsFixture)
        {
            _customerTestsFixture = customerTestsFixture;
        }
        

        [Fact(DisplayName = "New Valid Customer")]
        [Trait("Category", "Customer Fixture Tests")]
        public void Customer_NewCustomer_ShouldBeValid()
        {
            // Arrange
            var customer = _customerTestsFixture.GenerateValidCustomer();

            // Act
            var result = customer.IsValid();

            // Assert 
            Assert.True(result);
            Assert.Equal(0, customer.ValidationResult.Errors.Count);
        }
    }
}