using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class Order
    {
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
            if (orderItems.Any(product => product.ProductId == orderItem.ProductId))
            {
                var existingItem = orderItems.FirstOrDefault(product => product.ProductId == orderItem.ProductId);
                existingItem.AddQuantity(orderItem.Quantity);
                orderItem = existingItem;

                orderItems.Remove(existingItem);
            }

            this.orderItems.Add(orderItem);            
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
