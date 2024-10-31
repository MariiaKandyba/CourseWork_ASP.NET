using DTOs.Orders;

namespace Client.Models
{
    public class OrderViewModel
    {
        public string FirstName { get; set; } // Ім'я користувача
        public string LastName { get; set; } // Прізвище користувача
        public string Email { get; set; } // Електронна адреса користувача
        public AddressDto DeliveryAddress { get; set; } // Адреса доставки
        public decimal TotalPrice {get; set; }

        public List<ProductCartViewModel> Items { get; set; } = new(); // Список товарів у кошику

    }
}
