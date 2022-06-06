using Microsoft.AspNetCore.Mvc.Testing;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
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
        public readonly AppStoreFactory<TStartup> Factory;
        public HttpClient Client;


        public IntegrationTestsFixture()
        {
            var clientOptions = new WebApplicationFactoryClientOptions
            {

            };

            Factory = new AppStoreFactory<TStartup>();
            Client = Factory.CreateClient(clientOptions);
        }

        public void Dispose()
        {
            Client.Dispose();
            Factory.Dispose();
        }
    }
}
