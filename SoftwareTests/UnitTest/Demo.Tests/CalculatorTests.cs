using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Demo.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Calculator_Sum_ReturnSumValue()
        {
            // Arrange
            var calculator = new Calculator();

            //Act
            var result = calculator.Sum(5, 10);

            //Assert
            Assert.Equal(15, result);
        }

        [Theory]
        [InlineData(2,2,4)]
        [InlineData(5,10,15)]
        [InlineData(8,7,15)]
        public void Calculator_Sum_ReturnCorrectSumValues(double v1, double v2, double total)
        {
            // Arrange
            var calculator = new Calculator();

            //Act
            var result = calculator.Sum(v1, v2);

            //Assert
            Assert.Equal(total, result);
        }
    }
}
