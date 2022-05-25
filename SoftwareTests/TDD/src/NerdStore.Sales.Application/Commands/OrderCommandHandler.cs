using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
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
            if (!ValidateCommand(message)) return false;

            var order = await orderRepository.GetDraftedOrderByCustomerId(message.CustomerId);
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Quantity, message.UnitValue);

            if (order == null)
            {
                order = Order.OrderFactory.NewDraftOrder(message.CustomerId);
                order.InsertItem(orderItem);
                orderRepository.Insert(order);
            }
            else
            {
                var existsItemOrder = order.ExistsOrderItem(orderItem);
                order.InsertItem(orderItem);

                if (existsItemOrder)
                {
                    orderRepository.UpdateItem(order.OrderItems.FirstOrDefault(item => item.ProductId == orderItem.ProductId));
                }
                else
                {
                    orderRepository.InsertItem(orderItem);
                }

                orderRepository.Update(order);
            }

            order.InsertEvent(new InsertedOrderItemEvent(order.CustomerId, order.Id, message.ProductId, 
                message.ProductName, message.Quantity, message.UnitValue));

            return await orderRepository.UnitOfWork.Commit();
        }

        private bool ValidateCommand(Command message)
        {
            if (message.IsValid()) return true;

            foreach (var error in message.ValidationResult.Errors)
            {
                mediator.Publish(new DomainNotification(message.MessageType, error.ErrorMessage));
            }

            return false;
        }
    }
}
