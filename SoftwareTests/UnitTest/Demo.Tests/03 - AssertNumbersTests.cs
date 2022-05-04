using Xunit;

namespace Demo.Tests
{
    public class AssertNumbersTests
    {
        [Fact]
        public void Calculator_Sum_ShouldBeEqual()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Sum(1, 2);

            // Assert
            Assert.Equal(3, result);
        }
        
        [Fact]
        public void Calculator_Sum_ShouldNotBeEqual()
        {
            // Arrange
            var calculator = new Calculator();

            // Act
            var result = calculator.Sum(1.13123123123, 2.2312313123);

            // Assert
            Assert.NotEqual(3.3, result,1);
        }
    }
}