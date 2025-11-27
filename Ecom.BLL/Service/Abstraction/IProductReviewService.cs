using Ecom.BLL.ModelVM.ProductReview;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ecom.BLL.Service.Abstraction
{
    public interface IProductReviewService
    {
        Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetAllAsync();

        Task<ResponseResult<ProductReviewGetVM>> GetByIdAsync(int id);
        Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetByProductIdAsync(int productId);

        Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetByBrandIdAsync(int brandId);
        Task<ResponseResult<IEnumerable<ProductReviewGetVM>>> GetByUserIdAsync(string userId);
        Task<ResponseResult<bool>> CreateAsync(string userId, ProductReviewCreateVM vm);
        Task<ResponseResult<bool>> UpdateAsync(string userId, ProductReviewUpdateVM vm);
        Task<ResponseResult<bool>> ToggleDeleteAsync(int id, string userModified);
    }
}
