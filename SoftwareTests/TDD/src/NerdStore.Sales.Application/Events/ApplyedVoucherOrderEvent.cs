using NerdStore.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Events
{
    public class ApplyedVoucherOrderEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid VoucherId { get; private set; }

        public ApplyedVoucherOrderEvent(Guid customerId, Guid orderId, Guid voucherId)
        {
            AggregateId = orderId;
            CustomerId = customerId;
            OrderId = orderId;
            VoucherId = voucherId;
        }
    }
}
