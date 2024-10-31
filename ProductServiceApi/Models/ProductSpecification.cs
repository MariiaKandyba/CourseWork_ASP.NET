namespace ProductServiceApi.Models
{
    public class ProductSpecification
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;

        // Navigation property
        public Product Product { get; set; } = null!;
    }
}
