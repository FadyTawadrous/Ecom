
namespace Ecom.BLL.ModelVM.Role
{
    /// <summary>
    /// The DTO used to manage which roles a specific user has.
    /// </summary>
    public class UserRolesVM
    {
        [Required]
        public string UserId { get; set; } = null!;
        public List<string> RoleNames { get; set; } = new List<string>();
    }
}
