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
        void Insert(Order order);
        void Update(Order order);
        Task<Order> GetDraftedOrderByCustomerId(Guid customerId);
        void InsertItem(OrderItem orderItem);
        void UpdateItem(OrderItem orderItem);
    }
}
