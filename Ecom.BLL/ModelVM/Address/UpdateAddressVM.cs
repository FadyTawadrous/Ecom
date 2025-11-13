using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.ModelVM.Address
{
    public class UpdateAddressVM
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string UpdatedBy { get; set; } = null!;
    }
}
