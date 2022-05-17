using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class Order
    {
        public Order()
        {
            orderItems = new List<OrderItem>();
        }

        public decimal Amount { get; private set; }

        private readonly List<OrderItem> orderItems;

        public IReadOnlyCollection<OrderItem> OrderItems => orderItems;

        public void AddItem(OrderItem orderItem)
        {
            this.orderItems.Add(orderItem);
            Amount = OrderItems.Sum(item => item.Quantity * item.Amount);
        }
    }
}
