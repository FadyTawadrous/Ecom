
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IAccountRepo
    {
        // Command Methods
        Task<bool> AddAsync(AppUser newUser, string password);


    }
}
