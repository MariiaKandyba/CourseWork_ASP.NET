using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Orders
{
    public class CreateOrderRequestDto
    {
        public int UserId { get; set; }
        public List<OrderItemDto> Items { get; set; } = new List<OrderItemDto>();
        public AddressDto DeliveryAddress { get; set; }
    }
}
