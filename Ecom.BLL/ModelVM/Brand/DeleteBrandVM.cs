using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.Brand
{
    public class DeleteBrandVM
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; } = string.Empty;
    }
}
