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
        [Fact(DisplayName = "Insert Item New Order with Success")]
        [Trait("Category", "Sales - Order Command Handler")]
        public async Task InsertOrderItem_NewOrder_ShouldRunWithSuccess()
        {
            //Arrenge
            var orderCommand = new InsertOrderItemCommand(Guid.NewGuid(),
                Guid.NewGuid(), "Product Test", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            //Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            //Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(repository => repository.Insert(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IMediator>().Verify(mediator => mediator.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
    }
}
