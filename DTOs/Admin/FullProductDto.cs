using DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Admin
{
    public class FullProductDto
    {
        public ProductDto Product { get; set; }
        public int StockQuantity {  get; set; }
    }
}
