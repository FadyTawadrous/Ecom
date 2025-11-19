
namespace Ecom.BLL.Service.Abstraction
{
    public interface ITokenService
    {
        Task<string> CreateToken(AppUser user);
    }
}
