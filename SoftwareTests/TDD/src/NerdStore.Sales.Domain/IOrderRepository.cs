using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public interface IOrderRepository
    {
        void Insert(Order order);
    }
}
