namespace ProductServiceApi.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; } // Assuming you have a User entity
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Product Product { get; set; } = null!;
    }
}
