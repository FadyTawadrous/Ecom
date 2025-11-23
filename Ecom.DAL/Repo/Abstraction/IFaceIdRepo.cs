
namespace Ecom.DAL.Repo.Abstraction
{
    public interface IFaceIdRepo
    {
        Task<FaceId?> GetByIdAsync(int id);
        Task<IEnumerable<FaceId>> GetByUserIdAsync(string userId);
        Task<bool> AddAsync(FaceId face);
        Task<bool> UpdateAsync(FaceId newFace);
        Task<List<AppUser>> GetAllUsersWithFacesAsync();
        Task<bool> DeleteAsync(string userId);
    }
}
