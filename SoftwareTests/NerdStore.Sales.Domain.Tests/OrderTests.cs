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
        [Trait("Category", "Order Tests")]
        public void AddOrderItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var order = new Order();
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", 3, 50);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(150, order.Amount);

        }
    }
}
