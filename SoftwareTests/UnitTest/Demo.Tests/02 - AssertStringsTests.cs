using Xunit;

namespace Demo.Tests
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringsTools_JoinNames_ReturnFullName()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var fullName = sut.Join("Gabriel", "Capoia");

            // Assert
            Assert.Equal("Gabriel Capoia", fullName);
        }

        [Fact]
        public void StringsTools_JoinNames_ShouldIgnoreCase()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var fullName = sut.Join("Gabriel", "Capoia");

            // Assert
            Assert.Equal("GABRIEL CAPOIA", fullName, true);
        }

        [Fact]
        public void StringsTools_JoinNames_ShouldContainString()
        {
            // Arrange
            var sut = new StringsTools();

            // Act            
            var fullName = sut.Join("Gabriel", "Capoia");

            // Assert
            Assert.Contains("riel", fullName);
        }


        [Fact]
        public void StringsTools_JoinNames_ShouldStartsWith()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var fullName = sut.Join("Gabriel", "Capoia");

            // Assert
            Assert.StartsWith("Gabr", fullName);
        }


        [Fact]
        public void StringsTools_JoinNames_ShouldEndsWith()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var fullName = sut.Join("Gabriel", "Capoia");

            // Assert
            Assert.EndsWith("poia", fullName);
        }


        [Fact]
        public void StringsTools_JoinNames_ValidadeRegularExpression()
        {
            // Arrange
            var sut = new StringsTools();

            // Act
            var fullName = sut.Join("Gabriel", "Capoia");

            // Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", fullName);
        }
    }
}