using Ecom.BLL.ModelVM.FaceId;

namespace Ecom.BLL.Service.Abstraction
{
    public interface IFaceIdService
    {
        Task<ResponseResult<FaceId>> RegisterFaceAsync(RegisterFaceIdVM model);
        Task<ResponseResult<bool>> UpdateFaceAsync(UpdateFaceIdVM model);
        Task<ResponseResult<IEnumerable<FaceId>>> VerifyFaceByUserIdAsync(string userId);
        Task<ResponseResult<string>> VerifyFaceLoginAsync(IFormFile image);
        Task<ResponseResult<bool>> DeleteFaceAsync(string userId);
    }
}
