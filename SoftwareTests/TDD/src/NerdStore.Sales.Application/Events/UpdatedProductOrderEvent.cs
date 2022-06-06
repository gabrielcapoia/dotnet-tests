using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events
{
    public class UpdatedProductOrderEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public int Quantity { get; private set; }

        public UpdatedProductOrderEvent(Guid customerId, Guid orderId, Guid productId, int quantity)
        {
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            Quantity = quantity;
        }
    }
}
