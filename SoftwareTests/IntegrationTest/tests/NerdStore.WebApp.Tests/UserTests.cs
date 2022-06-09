using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [Collection(nameof(IntegrationWebTestFixtureCollection))]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> testsFixture;

        public UserTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            this.testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Register Successfuly")]
        [Trait("Category", "Web Integration - User")]
        public async Task User_Register_ShouldRunWithSuccess()
        {
            //Arrange
            var initialResponse = await testsFixture.Client.GetAsync("/Identity/Account/Register");
            initialResponse.EnsureSuccessStatusCode();

            //Act
            //Assert
        }
    }
}
