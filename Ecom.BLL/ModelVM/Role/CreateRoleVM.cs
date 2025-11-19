
namespace Ecom.BLL.ModelVM.Role
{
    public class CreateRoleVM
    {
        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string RoleName { get; set; } = null!;
    }
}
