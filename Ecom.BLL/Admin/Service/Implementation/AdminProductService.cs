
using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.BLL.ModelVM.Product;

namespace Ecom.BLL.Admin.Service.Implementation
{
    public class AdminProductService : IAdminProductService
    {
        private readonly IProductService _productService;

        public AdminProductService(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<ResponseResult<IEnumerable<GetProductVM>>> GetAllAsync()
            => await _productService.GetAllForAdminAsync();

        public async Task<ResponseResult<bool>> CreateAsync(CreateProductVM model)
            => await _productService.CreateAsync(model);

        public async Task<ResponseResult<bool>> UpdateAsync(UpdateProductVM model)
            => await _productService.UpdateAsync(model);

        public async Task<ResponseResult<bool>> DeleteAsync(int id)
            => await _productService.DeleteAsync(new DeleteProductVM { Id = id });

        public async Task<ResponseResult<bool>> IncreaseStockAsync(int productId, int quantity)
            => await _productService.IncreaseStockAsync(productId, quantity);

        public async Task<ResponseResult<bool>> DecreaseStockAsync(int productId, int quantity)
            => await _productService.DecreaseStockAsync(productId, quantity);
    }
}

