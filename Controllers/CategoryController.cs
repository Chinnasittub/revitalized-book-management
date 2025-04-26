using BookManagementPrj.Data;
using BookManagementPrj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagementPrj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly BookManagementContext db;

        public CategoryController(BookManagementContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] Category request)
        {
            try
            {
                var category = new Category
                {
                    CategoryName = request.CategoryName
                };

                db.Categories.Add(category);
                await db.SaveChangesAsync();

                return Ok("Category created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await db.Categories.Include(c => c.Books).FirstOrDefaultAsync(c => c.CategoryID == id);
                if (category == null) return NotFound("Category not found.");
                return Ok(category);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("max-id")]
        public async Task<IActionResult> GetMaxCategoryId()
        {
            try
            {
                var maxId = await db.Categories.MaxAsync(u => (int?)u.CategoryID);
                if (maxId == null) return NotFound("No categories found.");
                return Ok(maxId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            try
            {
                var categories = await db.Categories.Include(c => c.Books).ToListAsync();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] Category request)
        {
            try
            {
                var category = await db.Categories.FindAsync(id);
                if (category == null) return NotFound("Category not found.");

                category.CategoryName = request.CategoryName;

                db.Categories.Update(category);
                await db.SaveChangesAsync();

                return Ok("Category updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                var category = await db.Categories.FindAsync(id);
                if (category == null) return NotFound("Category not found.");

                db.Categories.Remove(category);
                await db.SaveChangesAsync();

                return Ok("Category deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}