using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.ProductReview
{
    public class DeletedVM
    {
        public int Id { get; set; }
        public string DeletedBy { get; set; } = "system";
        public bool IsRestore { get; set; } = false; // optional flag (service uses Toggle)
    }
}
