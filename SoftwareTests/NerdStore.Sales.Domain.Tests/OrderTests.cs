using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add Item New Order")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", 3, 50);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(150, order.Amount);
        }

        [Fact(DisplayName = "Add Existing Item Order")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_ExistingItem_ShouldIncrementQuantityAndSumValues()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test", 3, 50);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Order Test", 1, 50);

            //Act
            order.AddItem(orderItem2);

            //Asert
            Assert.Equal(200, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(4, order.OrderItems.FirstOrDefault(product => product.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Add Over Allowed Item Order")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_OverAllowedItems_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test", Order.MAX_ITEM_UNITS + 1, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem));
        }

        [Fact(DisplayName = "Add Existing Item Over Allowed Item Order")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_ExistingItemSumOverAllowedItems_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test", 1, 50);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Order Test", Order.MAX_ITEM_UNITS, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }
    }
}
