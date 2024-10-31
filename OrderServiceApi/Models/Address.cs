using DTOs.Orders;
using Newtonsoft.Json;

namespace OrderServiceApi.Models
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
}
