using Microsoft.EntityFrameworkCore;
using NerdStore.Core.Data;
using NerdStore.Sales.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly SalesContext _context;
        public IUnitOfWork UnitOfWork => _context;

        public OrderRepository(SalesContext context)
        {
            _context = context;
        }

        public void DeleteItem(OrderItem orderItem)
        {
            _context.OrderItems.Remove(orderItem);
        }

        public async Task<Order> GetDraftedOrderByCustomerId(Guid customerId)
        {
            var pedido = await _context.Orders.FirstOrDefaultAsync(p => p.CustomerId == customerId && p.OrderStatus == OrderStatus.Draft);
            if (pedido == null) return null;

            await _context.Entry(pedido)
                .Collection(i => i.OrderItems).LoadAsync();

            if (pedido.VoucherId != null)
            {
                await _context.Entry(pedido)
                    .Reference(i => i.Voucher).LoadAsync();
            }

            return pedido;
        }

        public Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId)
        {
            throw new NotImplementedException();
        }

        public Task<OrderItem> GetOrderItemByOrder(Guid orderId, Guid produtId)
        {
            throw new NotImplementedException();
        }

        public Task<Voucher> GetVoucherByCode(string code)
        {
            throw new NotImplementedException();
        }

        public void Insert(Order order)
        {
            throw new NotImplementedException();
        }

        public void InsertItem(OrderItem orderItem)
        {
            throw new NotImplementedException();
        }

        public void Update(Order order)
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(OrderItem orderItem)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
