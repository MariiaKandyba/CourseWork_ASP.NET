using DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Admin
{
    public class ProductCreateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public int BrandId { get; set; }
        public List<ImageDto> Images { get; set; }
        public int StockQuantity { get; set; }

    }
}
