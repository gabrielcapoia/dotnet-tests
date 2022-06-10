using Features.Tests;
using NerdStore.WebApp.MVC;
using NerdStore.WebApp.MVC.Models;
using NerdStore.WebApp.Tests.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.WebApp.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
    [Collection(nameof(IntegrationApiTestFixtureCollection))]
    public class OrderApiTests
    {
        private readonly IntegrationTestsFixture<StartupApiTests> testsFixture;

        public OrderApiTests(IntegrationTestsFixture<StartupApiTests> testsFixture)
        {
            this.testsFixture = testsFixture;
        }

        [Fact(DisplayName = "Insert item new order")]
        [Trait("Category", "API Integration - Order")]
        public async Task InsertItem_NewOrder_ShouldReturnSuccess()
        {
            // Arrange
            var itemInfo = new ItemViewModel
            {
                Id = new Guid("2eed0028-2603-49d5-8296-0d6e90af04ec"),
                Quantidade = 2
            };

            await testsFixture.RunLoginApi();
            testsFixture.Client.SetToken(testsFixture.UserToken);

            // Act
            var postResponse = await testsFixture.Client.PostAsJsonAsync("api/carrinho", itemInfo);

            // Assert
            postResponse.EnsureSuccessStatusCode();
        }

        [Fact(DisplayName = "Remover item em pedido existente"), TestPriority(2)]
        [Trait("Categoria", "Integração API - Pedido")]
        public async Task RemoverItem_PedidoExistente_DeveRetornarComSucesso()
        {
            // Arrange
            var produtoId = new Guid("2eed0028-2603-49d5-8296-0d6e90af04ec");
            await testsFixture.RunLoginApi();
            testsFixture.Client.SetToken(testsFixture.UserToken);

            // Act
            var deleteResponse = await testsFixture.Client.DeleteAsync($"api/carrinho/{produtoId}");

            // Assert
            deleteResponse.EnsureSuccessStatusCode();
        }
    }
}
