using NerdStore.WebApp.MVC;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.WebApp.Tests
{
    public class UserTests
    {
        private readonly IntegrationTestsFixture<StartupWebTests> testsFixture;

        public UserTests(IntegrationTestsFixture<StartupWebTests> testsFixture)
        {
            this.testsFixture = testsFixture;
        }
    }
}
