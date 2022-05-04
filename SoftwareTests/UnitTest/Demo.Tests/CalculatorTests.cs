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
    }
}
