using Features.Customers;
using System;
using Xunit;

namespace Features.Tests
{
    public class CustomerTests
    {
        [Fact(DisplayName = "New Valid Customer")]
        [Trait("Category", "Customer Trait Tests")]
        public void Customer_NewCustomer_ShouldBeValid()
        {
            // Arrange
            var customer = new Customer(
                Guid.NewGuid(),
                "Gabriel",
                "Capoia",
                DateTime.Now.AddYears(-30),
                "gabriel@capoia.com",
                true,
                DateTime.Now);

            // Act
            var result = customer.IsValid();

            // Assert 
            Assert.True(result);
            Assert.Equal(0, customer.ValidationResult.Errors.Count);
        }

        [Fact(DisplayName = "New Invalid Customer")]
        [Trait("Category", "Customer Trait Tests")]
        public void Customer_NewCustomer_ShouldBeInvalid()
        {
            // Arrange
            var customer = new Customer(
                Guid.NewGuid(),
                "",
                "",
                DateTime.Now,
                "a2b.com",
                true,
                DateTime.Now);

            // Act
            var result = customer.IsValid();

            // Assert 
            Assert.False(result);
            Assert.NotEqual(0, customer.ValidationResult.Errors.Count);
        }
    }
}