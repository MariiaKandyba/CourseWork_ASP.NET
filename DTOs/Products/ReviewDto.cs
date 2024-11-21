using DTOs.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs.Products
{
    public class ReviewDto
    {
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime ReviewDate { get; set; }
        public int? UserId { get; set; }
        public int ProductId { get; set; }

       
    }
}
