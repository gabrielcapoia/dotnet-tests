using NerdStore.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public class OrderItem : Entity
    {
        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            if (quantity < Order.MIN_ITEM_UNITS) throw new DomainException($"Min {Order.MIN_ITEM_UNITS} units per product");

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        internal void AddQuantity(int quantity)
        {
            Quantity += quantity;
        }

        internal void UpdateQuantity(int quantity)
        {
            Quantity = quantity;
        }

        internal void AssociateOrder(Guid orderId)
        {
            OrderId = orderId;
        }

        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        public decimal Amount => Quantity * UnitValue;

        public Guid OrderId { get; private set; }

        public Order Order { get; set; }
    }
}
