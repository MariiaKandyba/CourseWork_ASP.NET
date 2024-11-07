using DTOs.Orders;
using DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Admin
{
    public class FullOrderDto
    {
        public int Id { get; set; }
        public List<FullOrderItemDto> Items { get; set; }
        public UserDto User { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
        public AddressDto DeliveryAddress { get; set; }
        public decimal TotalPrice { get; set; }

    }
}
