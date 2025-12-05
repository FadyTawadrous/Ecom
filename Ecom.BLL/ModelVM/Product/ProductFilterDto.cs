using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.Product
{
    public class ProductFilterDto
    {

        public int? BrandId { get; set; }
        public int? CategoryId { get; set; }

        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public double? MinRating { get; set; }
        public string? Search { get; set; }
       
    }
}
