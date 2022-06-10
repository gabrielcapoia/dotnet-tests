using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
    [Collection(nameof(IntegrationWebTestFixtureCollection))]
    public class UserTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> testsFixture;

        public UserTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            this.testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Register Successfuly"), TestPriority(1)]
        [Trait("Category", "Web Integration - User")]
        public async Task User_Register_ShouldRunWithSuccess()
        {
            //Arrange
            var requestUri = "/Identity/Account/Register";
            testsFixture.GenerateUserPassword();

            var initialResponse = await testsFixture.Client.GetAsync(requestUri);
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = testsFixture.GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                { testsFixture.AntiForgeryFieldName, antiForgeryToken},
                {"Input.Email", testsFixture.UserEmail},
                {"Input.Password", testsFixture.UserPassword},
                {"Input.ConfirmPassword", testsFixture.UserPassword}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, requestUri) 
            {
                Content = new FormUrlEncodedContent(formData)
            };

            //Act
            var postResponse = await testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResponse.EnsureSuccessStatusCode();

            var responseString = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains($"Hello {testsFixture.UserEmail}!", responseString);
        }

        [Fact(DisplayName = "Register with error"), TestPriority(3)]
        [Trait("Category", "Web Integration - User")]
        public async Task User_RegisterWithWeakPassword_ShouldRunWithError()
        {
            //Arrange
            var requestUri = "/Identity/Account/Register";
            testsFixture.GenerateUserPassword();
            const string weakPass = "123456";

            var initialResponse = await testsFixture.Client.GetAsync(requestUri);
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = testsFixture.GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                { testsFixture.AntiForgeryFieldName, antiForgeryToken},
                {"Input.Email", testsFixture.UserEmail},
                {"Input.Password", weakPass},
                {"Input.ConfirmPassword", weakPass}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            //Act
            var postResponse = await testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResponse.EnsureSuccessStatusCode();

            var responseString = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains("Passwords must have at least one non alphanumeric character.", responseString);
            Assert.Contains("Passwords must have at least one lowercase (&#x27;a&#x27;-&#x27;z&#x27;).", responseString);
            Assert.Contains("Passwords must have at least one uppercase (&#x27;A&#x27;-&#x27;Z&#x27;).", responseString);
        }

        [Fact(DisplayName = "Login with success"), TestPriority(2)]
        [Trait("Category", "Web Integration - User")]
        public async Task User_Login_ShouldRunWithSuccess()
        {
            //Arrange
            var requestUri = "/Identity/Account/Login";
            var initialResponse = await testsFixture.Client.GetAsync(requestUri);
            initialResponse.EnsureSuccessStatusCode();

            var antiForgeryToken = testsFixture.GetAntiForgeryToken(await initialResponse.Content.ReadAsStringAsync());

            var formData = new Dictionary<string, string>
            {
                { testsFixture.AntiForgeryFieldName, antiForgeryToken},
                {"Input.Email", testsFixture.UserEmail},
                {"Input.Password", testsFixture.UserPassword}
            };

            var postRequest = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new FormUrlEncodedContent(formData)
            };

            //Act
            var postResponse = await testsFixture.Client.SendAsync(postRequest);

            //Assert
            postResponse.EnsureSuccessStatusCode();

            var responseString = await postResponse.Content.ReadAsStringAsync();
            Assert.Contains($"Hello {testsFixture.UserEmail}!", responseString);
        }
    }
}
