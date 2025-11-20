
namespace Ecom.BLL.ModelVM.Address
{
    public class CreateAddressVM
    {
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string AppUserId { get; set; } = null!;
        public string CreatedBy { get; set; } = null!;
    }
}
