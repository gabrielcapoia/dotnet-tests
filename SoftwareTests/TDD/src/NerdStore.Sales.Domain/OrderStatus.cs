using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NerdStore.Sales.Domain
{
    public enum OrderStatus
    {
        Draft = 0,
        Initiated = 1,
        Paid = 4,
        Delivered = 5,
        Canceled = 6
    }
}
