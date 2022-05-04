using Xunit;

namespace Demo.Tests
{
    public class AssertNullBoolTests
    {
        [Fact]
        public void Employee_Name_ShouldNotBeNullOrEmpty()
        {
            // Arrange & Act
            var employee = new Employee("", 1000);

            // Assert
            Assert.False(string.IsNullOrEmpty(employee.Name));
        }

        [Fact]
        public void Employee_Nickname_ShouldNotHaveNickname()
        {
            // Arrange & Act
            var employee = new Employee("Gabriel", 1000);

            // Assert
            Assert.Null(employee.Nickname);

            // Assert Bool
            Assert.True(string.IsNullOrEmpty(employee.Nickname));
            Assert.False(employee.Nickname?.Length > 0);
        }
    }
}