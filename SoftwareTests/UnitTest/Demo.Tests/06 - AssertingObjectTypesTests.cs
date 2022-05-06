using Xunit;

namespace Demo.Tests
{
    public class AssertingObjectTypesTests
    {
        [Fact]
        public void EmployeeFactory_Create_ShouldReturnEmployeeType()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Gabriel", 10000);

            // Assert
            Assert.IsType<Employee>(employee);
        }
        
        [Fact]
        public void EmployeeFactory_Create_ShouldReturnPersonDerivedType()
        {
            // Arrange & Act
            var employee = EmployeeFactory.Create("Gabriel", 10000);

            // Assert
            Assert.IsAssignableFrom<Person>(employee);
        }
    }
}