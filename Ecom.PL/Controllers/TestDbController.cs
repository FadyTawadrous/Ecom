using Ecom.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecom.PL.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestDbController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        // Use DI to get the DbContext
        public TestDbController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("brands")]
        public async Task<IActionResult> GetBrands()
        {
            // Fetch the brands
            var brands = await _context.Brands.ToListAsync();
            return Ok(brands);
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            // Fetch the categories
            var categories = await _context.Categories.ToListAsync();
            return Ok(categories);
        }
    }
}