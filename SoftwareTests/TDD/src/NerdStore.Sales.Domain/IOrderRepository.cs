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
    }
}
