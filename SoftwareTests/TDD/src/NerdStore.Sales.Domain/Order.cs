using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class Order
    {
        public static int MAX_ITEM_UNITS = 15;
        public static int MIN_ITEM_UNITS = 1;

        protected Order()
        {
            orderItems = new List<OrderItem>();
        }

        public Guid CustomerId { get; private set; }

        public decimal Amount => OrderItems.Sum(item => item.Amount);

        public OrderStatus OrderStatus { get; private set; }

        private readonly List<OrderItem> orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => orderItems;

        public void AddItem(OrderItem orderItem)
        {
            ValidateQuantityItemAllowed(orderItem);

            if (ExistsOrderItem(orderItem))
            {
                var existingItem = orderItems.FirstOrDefault(product => product.ProductId == orderItem.ProductId);

                existingItem.AddQuantity(orderItem.Quantity);
                orderItem = existingItem;

                orderItems.Remove(existingItem);
            }

            this.orderItems.Add(orderItem);
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

        public void SetAsDraft()
        {
            OrderStatus = OrderStatus.Draft;
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
