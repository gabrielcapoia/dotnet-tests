using MediatR;
using Moq;
using Moq.AutoMock;
using NerdStore.Sales.Application.Commands;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace NerdStore.Sales.Application.Tests.Orders
{
    public class OrderCommandHandlerTests
    {
        private readonly AutoMocker mocker;
        private readonly OrderCommandHandler orderHandler;
        private readonly Guid customerId;
        private readonly Guid productId;
        private readonly Order order;

        public OrderCommandHandlerTests()
        {
            mocker = new AutoMocker();
            orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            customerId = Guid.NewGuid();
            productId = Guid.NewGuid();

            order = Order.OrderFactory.NewDraftOrder(customerId);
        }

        [Fact(DisplayName = "Insert Item New Order with Success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task InsertOrderItem_NewOrder_ShouldRunWithSuccess()
        {
            //Arrenge
            var orderCommand = new InsertOrderItemCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Product Test", 2, 100);

            mocker.GetMock<IOrderRepository>()
                .Setup(repository => repository.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            //Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.Insert(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(mediator => mediator.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }

        [Fact(DisplayName = "Insert New Item Drafted Order with Success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task InsertOrderItem_NewItemDraftedOrder_ShouldRunWithSuccess()
        {
            //Arrenge
            var orderItem = new OrderItem(Guid.NewGuid(), "Order Test", 3, 50);
            order.InsertItem(orderItem);

            var orderCommand = new InsertOrderItemCommand(customerId,
                Guid.NewGuid(), "Product Test", 2, 100);
                        
            mocker.GetMock<IOrderRepository>()
                .Setup(repository => repository.GetDraftedOrderByCustomerId(customerId))
                .Returns(Task.FromResult(order));

            mocker.GetMock<IOrderRepository>()
                .Setup(repository => repository.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            //Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.InsertItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.Update(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Insert Existing Item Drafted Order with Success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task InsertOrderItem_ExistingItemDraftedOrder_ShouldRunWithSuccess()
        {
            //Arrenge
            var orderItem = new OrderItem(productId, "Product Test", 3, 50);
            order.InsertItem(orderItem);

            var orderCommand = new InsertOrderItemCommand(customerId,
                productId, "Product Test", 1, 50);
                        
            mocker.GetMock<IOrderRepository>()
                .Setup(repository => repository.GetDraftedOrderByCustomerId(customerId))
                .Returns(Task.FromResult(order));

            mocker.GetMock<IOrderRepository>()
                .Setup(repository => repository.UnitOfWork.Commit())
                .Returns(Task.FromResult(true));

            //Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.UpdateItem(It.IsAny<OrderItem>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.Update(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.UnitOfWork.Commit(), Times.Once);
        }

        [Fact(DisplayName = "Insert Item Invalid Command")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task InsertOrderItem_InvalidCommand_ShouldRunFalseAndThrowsNotificationEvents()
        {
            //Arrange
            var orderCommand = new InsertOrderItemCommand(Guid.Empty, Guid.Empty, string.Empty, 0, 0);
                        
            //Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.False(result);
            mocker.GetMock<IMediator>().Verify(mediator => mediator.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Exactly(5));
        }
    }
}
