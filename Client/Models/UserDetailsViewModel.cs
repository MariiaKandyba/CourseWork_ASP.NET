using DTOs.Orders;

namespace Client.Models
{
    public class UserDetailsViewModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public AddressDto Address{ get; set; }
    }
}
