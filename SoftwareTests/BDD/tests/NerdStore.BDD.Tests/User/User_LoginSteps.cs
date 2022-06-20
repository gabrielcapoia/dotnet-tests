using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class User_LoginSteps
    {
        private readonly UserLoginScreen _userLoginScreen;
        private readonly AutomacaoWebTestsFixture _testsFixture;

        public User_LoginSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _userLoginScreen = new UserLoginScreen(_testsFixture.BrowserHelper);
        }

        [When(@"he clicks login")]
        public void WhenHeClicksLogin()
        {
            // Act
            _userLoginScreen.ClicarNoLinkLogin();

            // Assert
            Assert.Contains(_testsFixture.Configuration.LoginUrl,
                _userLoginScreen.ObterUrl());
        }
        
        [When(@"fill in the login form data")]
        public void WhenFillInTheLoginFormData(Table table)
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "teste1@teste.com",
                Senha = "Teste@123"
            };
            _testsFixture.Usuario = usuario;

            // Act
            _userLoginScreen.PreencherFormularioLogin(usuario);

            // Assert
            Assert.True(_userLoginScreen.ValidarPreenchimentoFormularioLogin(usuario));
        }
        
        [When(@"click the login button")]
        public void WhenClickTheLoginButton()
        {
            _userLoginScreen.ClicarNoBotaoLogin();
        }
    }
}
