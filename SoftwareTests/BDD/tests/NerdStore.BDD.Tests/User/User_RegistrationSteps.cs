using NerdStore.BDD.Tests.Config;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.User
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class User_RegistrationSteps
    {
        private readonly AutomacaoWebTestsFixture _testsFixture;
        private readonly UserRegistrationScreen _userRegistrationScreen;

        public User_RegistrationSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _userRegistrationScreen = new UserRegistrationScreen(_testsFixture.BrowserHelper);
        }


        [When(@"He clicks register")]
        public void WhenHeClicksRegister()
        {
            //Arrange & Act
            _userRegistrationScreen.ClicarNoLinkRegistrar();

            //Assert
            Assert.Contains(_testsFixture.Configuration.RegisterUrl, _userRegistrationScreen.ObterUrl());
        }
        
        [When(@"fill in the form data")]
        public void WhenFillInTheFormData(Table table)
        {
            // Arrange
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;

            // Act
            _userRegistrationScreen.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_userRegistrationScreen.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [When(@"Click on the register button")]
        public void WhenClickOnTheRegisterButton()
        {
            _userRegistrationScreen.ClicarNoBotaoRegistrar();
        }
        
        [When(@"Fill the form data with a password without capitals")]
        public void WhenFillTheFormDataWithAPasswordWithoutCapitals(Table table)
        {
            // Arrange
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;
            usuario.Senha = "teste@123";

            // Act
            _userRegistrationScreen.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_userRegistrationScreen.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [When(@"Fill the form data with a password with no special character")]
        public void WhenFillTheFormDataWithAPasswordWithNoSpecialCharacter(Table table)
        {
            // Arrange
            _testsFixture.GerarDadosUsuario();
            var usuario = _testsFixture.Usuario;
            usuario.Senha = "Teste123";

            // Act
            _userRegistrationScreen.PreencherFormularioRegistro(usuario);

            // Assert
            Assert.True(_userRegistrationScreen.ValidarPreenchimentoFormularioRegistro(usuario));
        }
        
        [Then(@"He will get an error message that the password must contain an uppercase letter")]
        public void ThenHeWillGetAnErrorMessageThatThePasswordMustContainAnUppercaseLetter()
        {
            Assert.True(_userRegistrationScreen
                .ValidarMensagemDeErroFormulario("Passwords must have at least one uppercase ('A'-'Z')"));
        }
        
        [Then(@"He will get an error message that the password must contain a special character")]
        public void ThenHeWillGetAnErrorMessageThatThePasswordMustContainASpecialCharacter()
        {
            Assert.True(_userRegistrationScreen
                .ValidarMensagemDeErroFormulario("Passwords must have at least one non alphanumeric character"));
        }
    }
}
