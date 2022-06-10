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

        public async Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId)
        {
            return await _context.Orders.AsNoTracking().Where(p => p.CustomerId == customerId).ToListAsync();
        }

        public async Task<OrderItem> GetOrderItemByOrder(Guid orderId, Guid produtId)
        {
            return await _context.OrderItems.FirstOrDefaultAsync(p => p.ProductId == produtId && p.OrderId == orderId);
        }

        public async Task<Voucher> GetVoucherByCode(string code)
        {
            return await _context.Vouchers.FirstOrDefaultAsync(p => p.Code == code);
        }

        public void Insert(Order order)
        {
            _context.Orders.Add(order);
        }

        public void InsertItem(OrderItem orderItem)
        {
            _context.OrderItems.Add(orderItem);
        }

        public void Update(Order order)
        {
            _context.Orders.Update(order);
        }

        public void UpdateItem(OrderItem orderItem)
        {
            _context.OrderItems.Update(orderItem);
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
