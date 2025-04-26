using BookManagementPrj.Data;
using BookManagementPrj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagementPrj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly BookManagementContext db;

        public BookController(BookManagementContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBook([FromBody] Book request)
        {
            try
            {
                var book = new Book
                {
                    Title = request.Title,
                    Author = request.Author,
                    Description = request.Description,
                    Price = request.Price,
                    CategoryId = request.CategoryId
                };

                db.Books.Add(book);
                await db.SaveChangesAsync();

                return Ok("Book created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookById(int id)
        {
            try
            {
                var book = await db.Books.Include(b => b.Category).FirstOrDefaultAsync(b => b.BookId == id);
                if (book == null) return NotFound("Book not found.");
                return Ok(book);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBooks()
        {
            try
            {
                var books = await db.Books.Include(b => b.Category).ToListAsync();
                return Ok(books);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("max-id")]
        public async Task<IActionResult> GetMaxBookId()
        {
            try
            {
                var maxId = await db.Books.MaxAsync(u => (int?)u.BookId);
                if (maxId == null) return NotFound("No books found.");
                return Ok(maxId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBook(int id, [FromBody] Book request)
        {
            try
            {
                var book = await db.Books.FindAsync(id);
                if (book == null) return NotFound("Book not found.");

                book.Title = request.Title;
                book.Author = request.Author;
                book.Description = request.Description;
                book.Price = request.Price;
                book.CategoryId = request.CategoryId;

                db.Books.Update(book);
                await db.SaveChangesAsync();

                return Ok("Book updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            try
            {
                var book = await db.Books.FindAsync(id);
                if (book == null) return NotFound("Book not found.");

                db.Books.Remove(book);
                await db.SaveChangesAsync();

                return Ok("Book deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


    }
}
    