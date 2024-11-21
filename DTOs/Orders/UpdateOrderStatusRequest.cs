using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Orders
{
    public class UpdateOrderStatusRequest
    {
        public int OrderId { get; set; }
        public string NewStatus { get; set; }
    }
}
