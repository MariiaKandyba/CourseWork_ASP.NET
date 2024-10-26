using Newtonsoft.Json;

namespace ProductServiceApi.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }

        // Navigation property
        [JsonIgnore]
        public ICollection<Product> Products { get; set; } = new List<Product>();
    }
}
