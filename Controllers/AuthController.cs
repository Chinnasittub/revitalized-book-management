using BookManagementPrj.Data;
using BookManagementPrj.Models;
using BookManagementPrj.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookManagementPrj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly BookManagementContext db;

        public AuthController(BookManagementContext context, AuthService authService)
        {
            _authService = authService;
            db = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDto request)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Username == request.Username);
                if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password)) 
                    return Unauthorized("Invalid credentials.");

                var token = _authService.GenerateJwtToken(user);

                db.Tokens.Add(new Token
                {
                    TokenValue = token,
                    Expiration = DateTime.Now.AddMinutes(Convert.ToDouble(_authService.GetDurationInMinutes()))
                });
                await db.SaveChangesAsync();

                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while login process. " + ex.Message);
            }
        }

        [HttpPost("logout")]
        [Authorize] // Ensure the user is authenticated before allowing logout
        public async Task<IActionResult> Logout()
        {
            try
            {
                var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

                var storedToken = await db.Tokens.FirstOrDefaultAsync(t => t.TokenValue == token);
                if (storedToken == null) BadRequest("Invalid token.");

                db.Tokens.Remove(storedToken);
                await db.SaveChangesAsync();
                return Ok("User logged out successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An error occurred while logout process. " + ex.Message);
            }
        }
    }

    public class UserDto
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
