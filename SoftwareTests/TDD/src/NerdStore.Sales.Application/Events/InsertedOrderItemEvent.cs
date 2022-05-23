using MediatR;
using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events
{
    public class InsertedOrderItemEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        public InsertedOrderItemEvent(Guid customerId, Guid orderId, Guid productId, string productName, int quantity, decimal unitValue)
        {
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }
    }
}
