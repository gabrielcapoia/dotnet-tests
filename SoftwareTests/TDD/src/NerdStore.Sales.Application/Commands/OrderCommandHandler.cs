using MediatR;
using NerdStore.Sales.Application.Events;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NerdStore.Sales.Application.Commands
{
    public class OrderCommandHandler : IRequestHandler<InsertOrderItemCommand, bool>
    {
        private readonly IOrderRepository orderRepository;
        private readonly IMediator mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            this.orderRepository = orderRepository;
            this.mediator = mediator;
        }

        public async Task<bool> Handle(InsertOrderItemCommand message, CancellationToken cancellationToken)
        {
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Quantity, message.UnitValue);
            Order order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
            order.InsertItem(orderItem);

            orderRepository.Insert(order);

            await mediator
                .Publish(new InsertedOrderItemEvent(order.CustomerId, order.Id, message.ProductId, 
                message.ProductName, message.Quantity, message.UnitValue), cancellationToken);
            return true;
        }
    }
}
