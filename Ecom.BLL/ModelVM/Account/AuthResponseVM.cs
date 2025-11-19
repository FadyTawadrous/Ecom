
namespace Ecom.BLL.ModelVM.Account
{
    public class AuthResponseVM
    {
        // This is what we return after a successful Login or Register, we return the user and the JWT token
        public GetUserVM User { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime TokenExpiration { get; set; }
    }
}
