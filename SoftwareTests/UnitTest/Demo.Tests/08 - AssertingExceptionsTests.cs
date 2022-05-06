using System;
using Xunit;

namespace Demo.Tests
{
    public class AssertingExceptionsTests
    {
        [Fact]
        public void Calculator_Divide_ShouldReturnDivideByZeroError()
        {
            // Arrange
            var calculator = new Calculator();

            // Act & Assert
            Assert.Throws<DivideByZeroException>(() => calculator.Divide(10, 0));
        }


        [Fact]
        public void Employee_Salary_ShouldReturnSalaryLowerThanAllowed()
        {
            // Arrange & Act & Assert
            var exception =
                Assert.Throws<Exception>(() => EmployeeFactory.Create("Gabriel", 250));

            Assert.Equal("Salary lower than allowed", exception.Message);
        }
    }
}