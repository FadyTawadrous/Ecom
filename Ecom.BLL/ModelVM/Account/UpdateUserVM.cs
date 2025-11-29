
using System.Text.Json.Serialization;

namespace Ecom.BLL.ModelVM.Account
{
    public class UpdateUserVM
    {
        //[JsonIgnore]
        //public string Id { get; set; } = null!;

        [StringLength(100, MinimumLength = 2)]
        public string? DisplayName { get; set; }

        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
