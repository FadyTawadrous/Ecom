
namespace Ecom.BLL.Service.Abstraction
{
    public interface IAddressService
    {
        // Get
        Task<ResponseResult<GetAddressVM>> GetByIdAsync(int id);
        Task<ResponseResult<IEnumerable<GetAddressVM>>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<ResponseResult<IEnumerable<GetAddressVM>>> GetAllByUserIdAsync(string userId,
            int pageNumber = 1, int pageSize = 10);

        // Create
        Task<ResponseResult<bool>> CreateAsync(CreateAddressVM model);

        // Update
        Task<ResponseResult<bool>> UpdateAsync(UpdateAddressVM model);

        // Delete
        Task<ResponseResult<bool>> DeleteAsync(DeleteAddressVM model);

        // Get Delete Model
        Task<ResponseResult<DeleteAddressVM>> GetDeleteModelAsync(int id);
    }
}
