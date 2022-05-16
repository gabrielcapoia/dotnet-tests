using Xunit;

namespace Features.Tests
{
    public class TestFailedForEspecificReason
    {
        [Fact(DisplayName = "New Customer 2.0", Skip = "New version 2.0 breaking")]
        [Trait("Category", "Skiping tests")]
        public void Test_NotPassing_NewVersionBreaking()
        {
            Assert.True(false);
        }
    }
}