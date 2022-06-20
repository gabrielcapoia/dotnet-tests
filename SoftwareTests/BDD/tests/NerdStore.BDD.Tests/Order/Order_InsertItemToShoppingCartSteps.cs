using NerdStore.BDD.Tests.Config;
using NerdStore.BDD.Tests.User;
using System;
using TechTalk.SpecFlow;
using Xunit;

namespace NerdStore.BDD.Tests.Order
{
    [Binding]
    [CollectionDefinition(nameof(AutomacaoWebFixtureCollection))]
    public class Order_InsertItemToShoppingCartSteps
    {

        private readonly AutomacaoWebTestsFixture _testsFixture;
        private readonly OrderScreen _orderScreen;
        private readonly UserLoginScreen _userLoginScreen;

        private string _urlProduto;

        public Order_InsertItemToShoppingCartSteps(AutomacaoWebTestsFixture testsFixture)
        {
            _testsFixture = testsFixture;
            _orderScreen = new OrderScreen(testsFixture.BrowserHelper);
            _userLoginScreen = new UserLoginScreen(testsFixture.BrowserHelper);
        }

        [Given(@"The user is logged in")]
        public void GivenTheUserIsLoggedIn()
        {
            // Arrange
            var usuario = new Usuario
            {
                Email = "teste1@teste.com",
                Senha = "Teste@123"
            };
            _testsFixture.Usuario = usuario;

            // Act 
            var login = _userLoginScreen.Login(usuario);

            // Assert
            Assert.True(login);
        }

        [Given(@"A product is in o display")]
        public void GivenAProductIsInODisplay()
        {
            //Arrange           
            _orderScreen.AcessarVitrineDeProdutos();

            //Act
            _orderScreen.ObterDetalhesDoProduto();
            _urlProduto = _orderScreen.ObterUrl();

            // Assert
            Assert.True(_orderScreen.ValidarProdutoDisponivel());
        }
        
        [Given(@"It is available in stock")]
        public void GivenItIsAvailableInStock()
        {
            // Assert
            Assert.True(_orderScreen.ObterQuantidadeNoEstoque() > 0);
        }

        [When(@"the user insert a item to the cart")]
        public void WhenTheUserInsertAItemToTheCart()
        {
            // Act 
            _orderScreen.ClicarEmComprarAgora();
        }

        [Then(@"the user will be redirect to purchase summary")]
        public void ThenTheUserWillBeRedirectToPurchaseSummary()
        {
            // Assert
            Assert.True(_orderScreen.ValidarSeEstaNoCarrinhoDeCompras());
        }

        [Then(@"The order amount is equal to the inserted item amount")]
        public void ThenTheOrderAmountIsEqualToTheInsertedItemAmount()
        {
            // Arrange
            var valorUnitario = _orderScreen.ObterValorUnitarioProdutoCarrinho();
            var valorCarrinho = _orderScreen.ObterValorTotalCarrinho();

            // Assert
            Assert.Equal(valorUnitario, valorCarrinho);
        }

        [When(@"User adds an item above the maximum allowed quantity")]
        public void WhenUserAddsAnItemAboveTheMaximumAllowedQuantity()
        {
            // Arrange 
            _orderScreen.ClicarAdicionarQuantidadeItens(Sales.Domain.Order.MAX_ITEM_UNITS + 1);

            // Act
            _orderScreen.ClicarEmComprarAgora();
        }

        [Then(@"you will receive an error message mentioning that the limit amount has been exceeded")]
        public void ThenYouWillReceiveAnErrorMessageMentioningThatTheLimitAmountHasBeenExceeded()
        {
            // Arrange
            var mensagem = _orderScreen.ObterMensagemDeErroProduto();

            // Assert
            Assert.Contains($"The maximum quantity of an item is {Sales.Domain.Order.MAX_ITEM_UNITS}", mensagem);
        }

        [Given(@"The same product has already been added to the cart previously")]
        public void GivenTheSameProductHasAlreadyBeenAddedToTheCartPreviously()
        {
            // Act
            _orderScreen.NavegarParaCarrinhoDeCompras();
            _orderScreen.ZerarCarrinhoDeCompras();
            _orderScreen.AcessarVitrineDeProdutos();
            _orderScreen.ObterDetalhesDoProduto();
            _orderScreen.ClicarEmComprarAgora();

            // Assert
            Assert.True(_orderScreen.ValidarSeEstaNoCarrinhoDeCompras());

            _orderScreen.VoltarNavegacao();
        }

        [Then(@"The number of items for that product will have been increased by one more unit")]
        public void ThenTheNumberOfItemsForThatProductWillHaveBeenIncreasedByOneMoreUnit()
        {
            // Assert
            Assert.True(_orderScreen.ObterQuantidadeDeItensPrimeiroProdutoCarrinho() == 2);
        }

        [Then(@"The total value of the order will be the multiplication of the quantity of items by the unit value")]
        public void ThenTheTotalValueOfTheOrderWillBeTheMultiplicationOfTheQuantityOfItemsByTheUnitValue()
        {
            // Arrange
            var valorUnitario = _orderScreen.ObterValorUnitarioProdutoCarrinho();
            var valorCarrinho = _orderScreen.ObterValorTotalCarrinho();
            var quantidadeUnidades = _orderScreen.ObterQuantidadeDeItensPrimeiroProdutoCarrinho();

            // Assert
            Assert.Equal(valorUnitario * quantidadeUnidades, valorCarrinho);
        }

        [When(@"The user adds the maximum amount allowed to the cart")]
        public void WhenTheUserAddsTheMaximumAmountAllowedToTheCart()
        {
            // Arrange 
            _orderScreen.ClicarAdicionarQuantidadeItens(Sales.Domain.Order.MAX_ITEM_UNITS);

            // Act
            _orderScreen.ClicarEmComprarAgora();
        }
    }
}
