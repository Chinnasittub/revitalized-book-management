using BookManagementPrj.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace BookManagementPrj.Middleware
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public TokenValidationMiddleware(RequestDelegate next, IServiceScopeFactory serviceScopeFactory)
        {
            _next = next;
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Only check authenticated requests
            if (context.User.Identity?.IsAuthenticated == true)
            {
                // Extract the token from the Authorization header
                string authHeader = context.Request.Headers["Authorization"].ToString();
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
                {
                    string token = authHeader.Substring("Bearer ".Length).Trim();

                    // Check if the token exists in the database
                    using var scope = _serviceScopeFactory.CreateScope();
                    // Replace YourDbContext with your actual DbContext class name
                    var db = scope.ServiceProvider.GetRequiredService<BookManagementContext>();

                    var storedToken = await db.Tokens.FirstOrDefaultAsync(t => t.TokenValue == token);

                    // If token doesn't exist in the database or is expired, return 401 Unauthorized
                    if (storedToken == null)
                    {
                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("Invalid or revoked token");
                        return;
                    }

                    // Optionally, check if token is expired
                    if (storedToken.Expiration < DateTime.Now)
                    {
                        // Remove expired token from database
                        db.Tokens.Remove(storedToken);
                        await db.SaveChangesAsync();

                        context.Response.StatusCode = 401; // Unauthorized
                        await context.Response.WriteAsync("Token expired");
                        return;
                    }
                }
            }

            // Continue with the next middleware in the pipeline
            await _next(context);
        }
    }

    // Extension method to make it easier to register the middleware
    public static class TokenValidationMiddlewareExtensions
    {
        public static IApplicationBuilder UseTokenValidation(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenValidationMiddleware>();
        }
    }
}