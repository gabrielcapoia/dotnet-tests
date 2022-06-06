using NerdStore.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetListByCustomerId(Guid customerId);
        Task<Order> GetDraftedOrderByCustomerId(Guid customerId);
        void Insert(Order order);
        void Update(Order order);

        Task<OrderItem> GetOrderItemByOrder(Guid orderId, Guid produtId);
        void InsertItem(OrderItem orderItem);
        void UpdateItem(OrderItem orderItem);
        void DeleteItem(OrderItem orderItem);

        Task<Voucher> GetVoucherByCode(string code);
    }
}
