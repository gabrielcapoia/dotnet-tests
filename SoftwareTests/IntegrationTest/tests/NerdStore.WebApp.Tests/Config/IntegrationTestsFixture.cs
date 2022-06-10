using Bogus;
using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests.Config
{ 

    [CollectionDefinition(nameof(IntegrationWebTestFixtureCollection))]
    public class IntegrationWebTestFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupWebTests>>
    {

    }

    [CollectionDefinition(nameof(IntegrationApiTestFixtureCollection))]
    public class IntegrationApiTestFixtureCollection : ICollectionFixture<IntegrationTestsFixture<StartupApiTests>>
    {

    }


    public class IntegrationTestsFixture<TStartup> : IDisposable where TStartup : class
    {
        public string AntiForgeryFieldName = "__RequestVerificationToken";

        public readonly AppStoreFactory<TStartup> Factory;
        public HttpClient Client;

        public string UserEmail;
        public string UserPassword;

        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {

            };

            Factory = new AppStoreFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public string GetAntiForgeryToken(string htmlBody)
        {
            var requestVerificationTokenMatch =
                Regex.Match(htmlBody, $@"\<input name=""{AntiForgeryFieldName}"" type=""hidden"" value=""([^""]+)"" \/\>");

            if (requestVerificationTokenMatch.Success)
            {
                return requestVerificationTokenMatch.Groups[1].Captures[0].Value;
            }

            throw new ArgumentException($"Anti forgery token '{AntiForgeryFieldName}' not found", nameof(htmlBody));
        }

        public void GenerateUserPassword()
        {
            var faker = new Faker("pt_BR");
            UserEmail = faker.Internet.Email().ToLower();
            UserPassword = faker.Internet.Password(8, false, "", "@1Ab_");
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
