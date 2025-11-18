
namespace Ecom.BLL.ModelVM.Account
{
    public class RegisterUserVM
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;

        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string DisplayName { get; set; } = null!;

        // These are optional
        public IFormFile? ProfileImage { get; set; }
        public string? ProfileImageUrl { get; set; }
        public string? PhoneNumber { get; set; }

    }
}
