
namespace Ecom.BLL.ModelVM.Address
{
    public class GetAddressVM
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
    }
}
