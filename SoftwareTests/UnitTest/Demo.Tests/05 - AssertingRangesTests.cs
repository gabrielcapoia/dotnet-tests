using Xunit;
using Xunit.Abstractions;

namespace Demo.Tests
{
    public class AssertingRangesTests
    {
        [Theory]
        [InlineData(700)]
        [InlineData(1500)]
        [InlineData(2000)]
        [InlineData(7500)]
        [InlineData(8000)]
        [InlineData(15000)]
        public void Employee_Salary_SalaryRangeSholdRespectProfessionalLevel(double salary)
        {
            // Arrange & Act
            var employee = new Employee("Gabriel", salary);

            // Assert
            if (employee.ProfissionalLevel == ProfissionalLevel.Junior)
                Assert.InRange(employee.Salary, 500, 1999);

            if (employee.ProfissionalLevel == ProfissionalLevel.Mid)
                Assert.InRange(employee.Salary, 2000, 7999);

            if (employee.ProfissionalLevel == ProfissionalLevel.Senior)
                Assert.InRange(employee.Salary, 8000, double.MaxValue);

            Assert.NotInRange(employee.Salary, 0, 499);
        }
    }
}