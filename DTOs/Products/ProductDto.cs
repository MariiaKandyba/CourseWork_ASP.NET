using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Products
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsAvailable { get; set; }
        public CategoryDto Category { get; set; }
        public BrandDto Brand { get; set; }
        public List<SpecificationDto> Specifications { get; set; }
        public List<ImageDto> Images { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
