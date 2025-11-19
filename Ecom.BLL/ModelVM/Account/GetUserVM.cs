
namespace Ecom.BLL.ModelVM.Account
{
    public class GetUserVM
    {
        public string Id { get; set; } = null!;
        public string DisplayName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string? PhoneNumber { get; set; }
        public DateTime CreatedOn { get; set; }

    }
}
