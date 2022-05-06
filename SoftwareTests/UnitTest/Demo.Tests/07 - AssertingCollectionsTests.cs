using Xunit;

namespace Demo.Tests
{
    public class AssertingCollectionsTests
    {
        [Fact]
        public void Employee_Skills_ShouldNotHaveEmptySkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Gabriel", 10000);

            // Assert
            Assert.All(employee.Skills, skills => Assert.False(string.IsNullOrWhiteSpace(skills)));
        }

        [Fact]
        public void Employee_Skills_JuniorShouldHavaBasicSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Gabriel", 1000);

            // Assert
            Assert.Contains("OOP", employee.Skills);
        }


        [Fact]
        public void Employee_Skills_JuniorShouldNotHavaAdvancedSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Gabriel", 1000);

            // Assert
            Assert.DoesNotContain("Microservices", employee.Skills);
        }


        [Fact]
        public void Employee_Skills_SeniorSholdHaveAllSkills()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Gabriel", 15000);

            var basicSkills = new []
            {
                "Programming logic",
                "OOP",
                "Tests",
                "Microservices"
            };

            // Assert
            Assert.Equal(basicSkills, employee.Skills);
        }
    }
}