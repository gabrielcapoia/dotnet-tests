using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events
{
    public class DeletedProductOrderEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }

        public DeletedProductOrderEvent(Guid customerId, Guid orderId, Guid productId)
        {
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
        }
    }
}
