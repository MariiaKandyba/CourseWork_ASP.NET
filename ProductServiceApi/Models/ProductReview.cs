namespace ProductServiceApi.Models
{
    public class ProductReview
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime ReviewDate { get; set; } = DateTime.UtcNow;

        public Product Product { get; set; } = null!;
    }
}
