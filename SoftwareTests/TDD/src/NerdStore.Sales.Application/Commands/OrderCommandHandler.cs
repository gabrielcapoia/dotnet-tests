using MediatR;
using NerdStore.Core.DomainObjects;
using NerdStore.Core.Messages;
using NerdStore.Core.Messages.CommonMessages.Notifications;
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
    public class OrderCommandHandler : 
        IRequestHandler<InsertOrderItemCommand, bool>,
        IRequestHandler<UpdateOrderItemCommand, bool>,
        IRequestHandler<DeleteOrderItemCommand, bool>,
        IRequestHandler<ApplyVoucherOrderCommand, bool>
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

        public async Task<bool> Handle(UpdateOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await orderRepository.GetDraftedOrderByCustomerId(message.CustomerId);

            if (order == null)
            {
                await mediator.Publish(new DomainNotification("pedido", "Pedido não encontrado!"), cancellationToken);
                return false;
            }

            var orderItem = await orderRepository.GetOrderItemByOrder(order.Id, message.ProdutId);

            if (!order.ExistsOrderItem(orderItem))
            {
                await mediator.Publish(new DomainNotification("pedido", "Item do pedido não encontrado!"), cancellationToken);
                return false;
            }

            order.UpdateQuantity(orderItem, message.Quantity);
            order.InsertEvent(new UpdatedProductOrderEvent(message.CustomerId, order.Id, message.ProdutId, message.Quantity));

            orderRepository.UpdateItem(orderItem);
            orderRepository.Update(order);

            return await orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(DeleteOrderItemCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var order = await orderRepository.GetDraftedOrderByCustomerId(message.CustomerId);

            if (order == null)
            {
                await mediator.Publish(new DomainNotification("pedido", "Pedido não encontrado!"), cancellationToken);
                return false;
            }

            var orderItem = await orderRepository.GetOrderItemByOrder(order.Id, message.ProductId);

            if (orderItem != null && !order.ExistsOrderItem(orderItem))
            {
                await mediator.Publish(new DomainNotification("pedido", "Item do pedido não encontrado!"), cancellationToken);
                return false;
            }

            order.DeleteItem(orderItem);
            order.InsertEvent(new DeletedProductOrderEvent(message.CustomerId, order.Id, message.ProductId));

            orderRepository.DeleteItem(orderItem);
            orderRepository.Update(order);

            return await orderRepository.UnitOfWork.Commit();
        }

        public async Task<bool> Handle(ApplyVoucherOrderCommand message, CancellationToken cancellationToken)
        {
            if (!ValidateCommand(message)) return false;

            var pedido = await orderRepository.GetDraftedOrderByCustomerId(message.CustomerId);

            if (pedido == null)
            {
                await mediator.Publish(new DomainNotification("pedido", "Pedido não encontrado!"), cancellationToken);
                return false;
            }

            var voucher = await orderRepository.GetVoucherByCode(message.VoucherCode);

            if (voucher == null)
            {
                await mediator.Publish(new DomainNotification("pedido", "Voucher não encontrado!"), cancellationToken);
                return false;
            }

            var voucherAplicacaoValidation = pedido.ApplyVoucher(voucher);
            if (!voucherAplicacaoValidation.IsValid)
            {
                foreach (var error in voucherAplicacaoValidation.Errors)
                {
                    await mediator.Publish(new DomainNotification(error.ErrorCode, error.ErrorMessage), cancellationToken);
                }

                return false;
            }

            pedido.InsertEvent(new ApplyedVoucherOrderEvent(message.CustomerId, pedido.Id, voucher.Id));

            orderRepository.Update(pedido);

            return await orderRepository.UnitOfWork.Commit();
        }
    }
}
