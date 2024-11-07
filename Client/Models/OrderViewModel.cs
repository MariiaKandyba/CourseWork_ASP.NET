using DTOs.Orders;

namespace Client.Models
{
    public class OrderViewModel
    {
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string Email { get; set; } 
        public AddressDto DeliveryAddress { get; set; } 
        public decimal TotalPrice {get; set; }
        public string Status {get; set; }

        public List<ProductCartViewModel> Items { get; set; } = new(); 

    }
}
