using Ecom.BLL.ModelVM.Category;
using Ecom.BLL.Service.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecom.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _service;

        public CategoryController(ICategoryService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            if (result.IsSuccess) 
            {
                return Ok(result);
            }
            return StatusCode(500, result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _service.GetByIdAsync(id);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddCategoryVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.AddAsync(model);

            if (!result.IsSuccess)
                return Conflict(result);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> Update(UpdateCategoryVM model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateAsync(model);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpPatch("toggle-delete")]
        public async Task<IActionResult> ToggleDelete(DeleteCategoryVM model)
        {
            var result = await _service.DeleteAsync(model);

            if (!result.IsSuccess)
                return NotFound(result);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> HardDelete(int id)
        {
            var result = await _service.HardDeleteAsync(new DeleteCategoryVM { Id = id });

            if (!result.IsSuccess)
                return NotFound(result);

            return NoContent();
        }
    }
}
