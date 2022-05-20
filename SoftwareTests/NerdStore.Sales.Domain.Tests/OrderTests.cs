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

        [Fact(DisplayName = "Add Existing Order Item")]
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

        [Fact(DisplayName = "Add Over Allowed Order Item")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_OverAllowedItems_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", Order.MAX_ITEM_UNITS + 1, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem));
        }

        [Fact(DisplayName = "Add Existing Item Over Allowed Quantity Order Item")]
        [Trait("Category", "Sales - Order")]
        public void AddOrderItem_ExistingItemSumOverAllowedQuantityItems_ShouldReturnException()
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

        [Fact(DisplayName = "Update Non Existing Order Item")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_NonExistingOrderItem_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", 5, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(orderItem));
        }

        [Fact(DisplayName = "Update Valid Order Item")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_ValidOrderItem_ShouldUpdateQuantity()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test", 2, 50);
            order.AddItem(orderItem);

            var updatedOrderItem = new OrderItem(productId, "Order Test", 5, 50);
            var newQuantity = updatedOrderItem.Quantity;
            //Act
            order.UpdateItem(updatedOrderItem);

            //Assert
            Assert.Equal(newQuantity, order.OrderItems.FirstOrDefault(product => product.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Update Order Item Validate Order Amount")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_OrderWithDifferentItems_ShouldUpdateOrderAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            var orderItem1 = new OrderItem(Guid.NewGuid(), "Order Test 1", 2, 50);
            order.AddItem(orderItem1);

            Guid productId = Guid.NewGuid();
            var orderItem2 = new OrderItem(productId, "Order Test 2", 2, 40);
            order.AddItem(orderItem2);

            var updatedOrderItem = new OrderItem(productId, "Order Test", 5, 40);
            var orderAmount = orderItem1.Quantity * orderItem1.UnitValue +
                              updatedOrderItem.Quantity * updatedOrderItem.UnitValue;
            //Act
            order.UpdateItem(updatedOrderItem);

            //Assert
            Assert.Equal(orderAmount, order.Amount);
        }

        [Fact(DisplayName = "Update Order Item Over Allowed Quantity Order Item")]
        [Trait("Category", "Sales - Order")]
        public void UpdateOrderItem_OverAllowedQuantityItems_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test 2", 2, 40);
            order.AddItem(orderItem);

            var updatedOrderItem = new OrderItem(productId, "Order Test", Order.MAX_ITEM_UNITS + 1, 40);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(updatedOrderItem));
        }

        [Fact(DisplayName = "Delete Non Existing Order Item")]
        [Trait("Category", "Sales - Order")]
        public void DeleteOrderItem_NonExistingOrderItem_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", 5, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.DeleteItem(orderItem));
        }

        [Fact(DisplayName = "Delete Order Item Validate Order Amount")]
        [Trait("Category", "Sales - Order")]
        public void DeleteOrderItem_OrderWithDifferentItems_ShouldUpdateOrderAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            var orderItem1 = new OrderItem(Guid.NewGuid(), "Order Test 1", 2, 50);
            order.AddItem(orderItem1);

            Guid productId = Guid.NewGuid();
            var orderItem2 = new OrderItem(productId, "Order Test 2", 2, 40);
            order.AddItem(orderItem2);
                        
            var orderAmount = orderItem2.Quantity * orderItem2.UnitValue;
            //Act
            order.DeleteItem(orderItem1);

            //Assert
            Assert.Equal(orderAmount, order.Amount);
        }
    }
}
