using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace Features.Tests
{
    [Collection(nameof(CustomerAutoMockerCollection))]
    public class CustomerFluentAssertionsTests
    {
        private readonly CustomerTestsAutoMockerFixture _customerTestsFixture;
        readonly ITestOutputHelper _outputHelper;

        public CustomerFluentAssertionsTests(CustomerTestsAutoMockerFixture customerTestsFixture, 
                                            ITestOutputHelper outputHelper)
        {
            _customerTestsFixture = customerTestsFixture;
            _outputHelper = outputHelper;
        }


        [Fact(DisplayName = "New Valid Customer")]
        [Trait("Category", "Customer Fluent Assertion Tests")]
        public void Customer_NewCustomer_ShouldBeValid()        
        {
            // Arrange
            var customer = _customerTestsFixture.GenerateValidCustomer();

            // Act
            var result = customer.IsValid();

            // Assert 
            //Assert.True(result);
            //Assert.Equal(0, customer.ValidationResult.Errors.Count);

            // Assert 
            result.Should().BeTrue();
            customer.ValidationResult.Errors.Should().HaveCount(0);
        }

        [Fact(DisplayName = "New Invalid Customer")]
        [Trait("Category", "Customer Fluent Assertion Tests")]
        public void Customer_NewCustomer_ShouldBeInvalid()        
        {
            // Arrange
            var customer = _customerTestsFixture.GenerateInvalidCustomer();

            // Act
            var result = customer.IsValid();

            // Assert 
            //Assert.False(result);
            //Assert.NotEqual(0, customer.ValidationResult.Errors.Count);

            // Assert 
            result.Should().BeFalse();
            customer.ValidationResult.Errors.Should().HaveCountGreaterOrEqualTo(1, "must have validation errors");

            _outputHelper.WriteLine($"{customer.ValidationResult.Errors.Count} errors were found in this validation");
        }
    }
}