﻿using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Sales.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "New Item Order Below Allowed")]
        [Trait("Category", "Order Item Tests")]
        public void AddOrderItem_BellowllowedItems_ShouldReturnException()
        {
            //Arrange, Act & Assert
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Order Test", Order.MIN_ITEM_UNITS - 1, 50));
        }
    }
}
