using FluentValidation.Results;
using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class Order : Entity
    {
        public static int MAX_ITEM_UNITS = 15;
        public static int MIN_ITEM_UNITS = 1;

        protected Order()
        {
            orderItems = new List<OrderItem>();
        }

        public Guid CustomerId { get; private set; }

        public decimal Amount => CalculateAmount();
        public decimal DiscountAmount => CalculateDiscountAmount();
        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => orderItems;

        public bool HasVoucherApplyed => Voucher != null;

        public Voucher Voucher { get; private set; }

        public void SetAsDraft()
        {
            OrderStatus = OrderStatus.Draft;
        }

        public void InsertItem(OrderItem orderItem)
        {
            ValidateQuantityItemAllowed(orderItem);

            if (ExistsOrderItem(orderItem))
            {
                var existingItem = orderItems.FirstOrDefault(product => product.ProductId == orderItem.ProductId);

                existingItem.AddQuantity(orderItem.Quantity);
                orderItem = existingItem;

                orderItems.Remove(existingItem);
            }

            orderItems.Add(orderItem);
        }

        public void UpdateItem(OrderItem orderItem)
        {
            ValidateNonExistOrderItem(orderItem);
            ValidateQuantityItemAllowed(orderItem);

            var existingItem = orderItems.FirstOrDefault(product => product.ProductId == orderItem.ProductId);

            orderItems.Remove(existingItem);
            orderItems.Add(orderItem);
        }

        public void DeleteItem(OrderItem orderItem)
        {
            ValidateNonExistOrderItem(orderItem);

            orderItems.Remove(orderItem);
        }

        public ValidationResult ApplyVoucher(Voucher voucher)
        {
            var result = voucher.ValidateIfItIsApplicable();
            if (result.IsValid) Voucher = voucher;
            return result;
        }

        private void ValidateNonExistOrderItem(OrderItem orderItem)
        {
            if (!ExistsOrderItem(orderItem))
                throw new DomainException($"Order item does not exist in the order");
        }

        private bool ExistsOrderItem(OrderItem orderItem)
        {
            return orderItems.Any(product => product.ProductId == orderItem.ProductId);
        }

        private void ValidateQuantityItemAllowed(OrderItem orderItem)
        {
            var quantityItem = orderItem.Quantity;
            if (ExistsOrderItem(orderItem))
            {
                var existingItem = orderItems.FirstOrDefault(product => product.ProductId == orderItem.ProductId);
                quantityItem += existingItem.Quantity;
            }

            if (quantityItem > MAX_ITEM_UNITS) throw new DomainException($"Max {MAX_ITEM_UNITS} units per product");
        }

        private decimal CalculateAmount()
        {
            decimal totalAmount = OrderItems.Sum(item => item.Amount);
            decimal totalAmountWithDiscount = totalAmount - CalculateDiscountAmount();

            totalAmountWithDiscount = totalAmountWithDiscount < 0 ? 0 : totalAmountWithDiscount;

            return totalAmountWithDiscount;
        }

        private decimal CalculateDiscountAmount()
        {
            if (!HasVoucherApplyed) return 0;

            if (Voucher.DiscountTypeVoucher == DiscountTypeVoucher.Value && Voucher.DiscountAmount.HasValue)
                return Voucher.DiscountAmount.Value;

            if (Voucher.DiscountTypeVoucher == DiscountTypeVoucher.Percentage && Voucher.DiscountPercentage.HasValue)
                return (OrderItems.Sum(item => item.Amount) * Voucher.DiscountPercentage.Value) / 100;

            return 0;
        }

        public static class OrderFactory
        {
            public static Order NewDraftOrder(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId
                };

                order.SetAsDraft();
                return order;
            }
        }
    }
}
