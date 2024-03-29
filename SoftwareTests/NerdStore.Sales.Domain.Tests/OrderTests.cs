﻿using NerdStore.Core.DomainObjects;
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
        [Fact(DisplayName = "Insert Item New Order")]
        [Trait("Category", "Sales - Order")]
        public void InsertOrderItem_NewOrder_ShouldUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", 3, 50);

            // Act
            order.InsertItem(orderItem);

            // Assert
            Assert.Equal(150, order.Amount);
        }

        [Fact(DisplayName = "Insert Existing Order Item")]
        [Trait("Category", "Sales - Order")]
        public void InsertOrderItem_ExistingItem_ShouldIncrementQuantityAndSumValues()
        {
            // Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test", 3, 50);
            order.InsertItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Order Test", 1, 50);

            //Act
            order.InsertItem(orderItem2);

            //Asert
            Assert.Equal(200, order.Amount);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(4, order.OrderItems.FirstOrDefault(product => product.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Insert Over Allowed Order Item")]
        [Trait("Category", "Sales - Order")]
        public void InsertOrderItem_OverAllowedItems_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", Order.MAX_ITEM_UNITS + 1, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.InsertItem(orderItem));
        }

        [Fact(DisplayName = "Insert Existing Item Over Allowed Quantity Order Item")]
        [Trait("Category", "Sales - Order")]
        public void InsertOrderItem_ExistingItemSumOverAllowedQuantityItems_ShouldReturnException()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            Guid productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Order Test", 1, 50);
            order.InsertItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Order Test", Order.MAX_ITEM_UNITS, 50);

            //Act & Assert
            Assert.Throws<DomainException>(() => order.InsertItem(orderItem2));
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
            order.InsertItem(orderItem);

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
            order.InsertItem(orderItem1);

            Guid productId = Guid.NewGuid();
            var orderItem2 = new OrderItem(productId, "Order Test 2", 2, 40);
            order.InsertItem(orderItem2);

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
            order.InsertItem(orderItem);

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
            order.InsertItem(orderItem1);

            Guid productId = Guid.NewGuid();
            var orderItem2 = new OrderItem(productId, "Order Test 2", 2, 40);
            order.InsertItem(orderItem2);
                        
            var orderAmount = orderItem2.Quantity * orderItem2.UnitValue;
            //Act
            order.DeleteItem(orderItem1);

            //Assert
            Assert.Equal(orderAmount, order.Amount);
        }

        [Fact(DisplayName = "Apply valid voucher")]
        [Trait("Category", "Sales - Order")]
        public void Order_ApplyValidVoucher_ShouldReturnWithoutErros()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15", null, 10, 1,
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Percentage);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.True(result.IsValid);
        }

        [Fact(DisplayName = "Apply Invalid voucher")]
        [Trait("Category", "Sales - Order")]
        public void Order_ApplyInvalidVoucher_ShouldReturnWithErros()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());
            var voucher = new Voucher("PROMO-15", null, 10, 1,
                DateTime.Now.AddDays(-1), true, false, DiscountTypeVoucher.Percentage);

            //Act
            var result = order.ApplyVoucher(voucher);

            //Assert
            Assert.False(result.IsValid);
        }

        [Fact(DisplayName = "Apply type value voucher")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_TypeValueVoucher_ShouldDiscountFromOrderAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            var orderItem1 = new OrderItem(Guid.NewGuid(), "Order Test 1", 2, 50);
            order.InsertItem(orderItem1);

            var orderItem2 = new OrderItem(Guid.NewGuid(), "Order Test 2", 2, 40);
            order.InsertItem(orderItem2);

            var voucher = new Voucher("PROMO-15", 20, null, 1,
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Value);

            var amountWithDiscount = order.Amount - voucher.DiscountAmount;

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(amountWithDiscount, order.Amount);
        }

        [Fact(DisplayName = "Apply type percentage voucher")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_TypePercentageVoucher_ShouldDiscountFromOrderAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            var orderItem1 = new OrderItem(Guid.NewGuid(), "Order Test 1", 2, 50);
            order.InsertItem(orderItem1);

            var orderItem2 = new OrderItem(Guid.NewGuid(), "Order Test 2", 2, 40);
            order.InsertItem(orderItem2);

            var voucher = new Voucher("PROMO-15", null, 10, 1,
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Percentage);

            var discountAmount = (order.Amount * voucher.DiscountPercentage) / 100;
            var amountWithDiscount = order.Amount - discountAmount;

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(amountWithDiscount, order.Amount);
        }

        [Fact(DisplayName = "Discount Amount Greater Than Order Amount")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_DiscountAmountGreaterThanOrderAmount_OrderAmountShouldBeZero()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            var orderItem1 = new OrderItem(Guid.NewGuid(), "Order Test 1", 2, 50);
            order.InsertItem(orderItem1);

            var voucher = new Voucher("PROMO-15", 150, null, 1,
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Value);

            //Act
            order.ApplyVoucher(voucher);

            //Assert
            Assert.Equal(0, order.Amount);
        }

        [Fact(DisplayName = "Apply voucher recalculate Amount with discount on update order items")]
        [Trait("Category", "Sales - Order")]
        public void ApplyVoucher_UpdateOrderItems_ShouldDiscountOrderAmount()
        {
            //Arrange
            var order = Order.OrderFactory.NewDraftOrder(Guid.NewGuid());

            var orderItem1 = new OrderItem(Guid.NewGuid(), "Order Test 1", 2, 50);
            order.InsertItem(orderItem1);

            var voucher = new Voucher("PROMO-15", 40, null, 1,
                DateTime.Now.AddDays(15), true, false, DiscountTypeVoucher.Value);

            order.ApplyVoucher(voucher);

            var orderItem2 = new OrderItem(Guid.NewGuid(), "Order Test 2", 2, 40);
            
            //Act
            order.InsertItem(orderItem2);

            //Assert
            var amountWithDiscount = order.OrderItems.Sum(item => item.Amount) - voucher.DiscountAmount;
            Assert.Equal(amountWithDiscount, order.Amount);
        }
    }
}
