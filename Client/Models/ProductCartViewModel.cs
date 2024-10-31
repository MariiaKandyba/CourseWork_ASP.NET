namespace Client.Models
{
    public class ProductCartViewModel
    {
        public int Id { get; set; } // Ідентифікатор товару
        public string Name { get; set; } // Назва товару
        public decimal Price { get; set; } // Ціна товару
        public string ImageUrl { get; set; } // URL зображення товару
        public string Description { get; set; } // Опис товару
        public int Quantity { get; set; } // Кількість товару в кошику (з CartItem)
    }

}
