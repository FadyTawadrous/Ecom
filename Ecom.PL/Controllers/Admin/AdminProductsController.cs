using Ecom.BLL.Admin.Service.Abstraction;
using Ecom.BLL.ModelVM.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers.Admin
{
    [Route("api/admin/products")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AdminProductsController : ControllerBase
    {
        private readonly IAdminProductService _service;

        public AdminProductsController(IAdminProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductVM model)
        {
            var result = await _service.CreateAsync(model);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromForm] UpdateProductVM model)
        {
            var result = await _service.UpdateAsync(model);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("stock/increase")]
        public async Task<IActionResult> IncreaseStock([FromQuery] int productId, [FromQuery] int quantity)
        {
            var result = await _service.IncreaseStockAsync(productId, quantity);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }

        [HttpPut("stock/decrease")]
        public async Task<IActionResult> DecreaseStock([FromQuery] int productId, [FromQuery] int quantity)
        {
            var result = await _service.DecreaseStockAsync(productId, quantity);
            return result.IsSuccess ? Ok(result) : BadRequest(result);
        }
    }
}

