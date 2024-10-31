namespace OrderServiceApi.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int IdUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public int AddressId { get; set; }
        public Address Address { get; set; }
        public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
