using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class OrderItem
    {
        public OrderItem(Guid productId, string productName, int quantity, decimal amount)
        {
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            Amount = amount;
        }

        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal Amount { get; private set; }
    }
}
