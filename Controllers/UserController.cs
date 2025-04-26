using BookManagementPrj.Data;
using BookManagementPrj.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BookManagementPrj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly BookManagementContext db;

        public UserController(BookManagementContext context)
        {
            db = context;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] User request)
        {
            try
            {
                // Check if the username already exists
                if (await db.Users.AnyAsync(u => u.Username == request.Username)) return BadRequest("Username already exists.");

                var user = new User
                {
                    Username = request.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(request.Password),
                    Email = request.Email,
                };

                db.Users.Add(user);
                await db.SaveChangesAsync();

                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            try
            {
                var user = await db.Users.FindAsync(id);
                if (user == null) return NotFound("User not found.");
                return Ok(user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("max-id")]
        public async Task<IActionResult> GetMaxUserId()
        {
            try
            {
                var maxId = await db.Users.MaxAsync(u => (int?)u.UserID);
                if (maxId == null) return NotFound("No users found.");
                return Ok(maxId);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await db.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User request)
        {
            try
            {
                var user = await db.Users.FindAsync(id);
                if (user == null) return NotFound("User not found.");

                user.Username = request.Username;
                user.Password = BCrypt.Net.BCrypt.HashPassword(request.Password);
                user.Email = request.Email;

                db.Users.Update(user);
                await db.SaveChangesAsync();

                return Ok("User updated successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("remove-test-user")]
        public async Task<IActionResult> DeleteTestUser()
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == "testLoginUser");
                if (user == null) return NotFound("Test user not found.");
                db.Users.Remove(user);
                await db.SaveChangesAsync();
                return Ok("Test user deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                var user = await db.Users.FindAsync(id);
                if (user == null) return NotFound("User not found.");

                db.Users.Remove(user);
                await db.SaveChangesAsync();

                return Ok("User deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
