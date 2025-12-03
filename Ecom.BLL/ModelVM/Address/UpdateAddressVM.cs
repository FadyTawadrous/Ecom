
namespace Ecom.BLL.ModelVM.Address
{
    public class UpdateAddressVM
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string? PostalCode { get; set; }
        public string? UpdatedBy { get; set; } = null!;
        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }
}
