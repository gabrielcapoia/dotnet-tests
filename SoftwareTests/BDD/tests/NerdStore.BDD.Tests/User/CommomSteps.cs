using NerdStore.BDD.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class CommomSteps
    {
        private readonly AutomacaoWebTestsFixture _testsFixture;
        private readonly UserRegistrationScreen _userRegistrationScreen;

        public CommomSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _userRegistrationScreen = new UserRegistrationScreen(_testsFixture.BrowserHelper);
        }

        [Given(@"that the visitor is accessing the store's website")]
        public void GivenThatTheVisitorIsAccessingTheStoreSWebsite()
        {
            //Arrange & Act
            _userRegistrationScreen.AcessarSiteLoja();

            //Assert
            Assert.Contains(_testsFixture.Configuration.DomainUrl, _userRegistrationScreen.ObterUrl());
        }

        [Then(@"He will be redirected to the showcase")]
        public void ThenHeWillBeRedirectedToTheShowcase()
        {
            // Assert
            Assert.Equal(_testsFixture.Configuration.DomainUrl, _userRegistrationScreen.ObterUrl());
        }

        [Then(@"A greeting with your email will be displayed in the top menu")]
        public void ThenAGreetingWithYourEmailWillBeDisplayedInTheTopMenu()
        {
            // Assert
            Assert.True(_userRegistrationScreen.ValidarSaudacaoUsuarioLogado(_testsFixture.Usuario));
        }
    }
}
