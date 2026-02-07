using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;
using Concrete.Api.Data;
using Concrete.Api.Models;
using Concrete.Api.Models.Auth;
using Concrete.Api.Services; // <-- Add this using directive if IJwtService is in this namespace

namespace Concrete.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IJwtService _jwtService; // Change type from object to IJwtService

    public AuthController(AppDbContext context, IJwtService jwtService) // Add IJwtService to constructor
    {
        _context = context;
        _jwtService = jwtService; // Assign injected jwtService
    }

    // REGISTER
    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var exists = await _context.Users
            .AnyAsync(u => u.Username == request.Username);

        if (exists)
            return BadRequest("Username already exists");

        var user = new User
        {
            Username = request.Username,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        return Ok("Register success");
    }

    // LOGIN
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Username == request.Username);

        if (user == null)
            return Unauthorized("Invalid credentials");

        if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            return Unauthorized("Invalid credentials");

        var token = _jwtService.GenerateToken(user.Id, user.Username);

        return Ok(new
        {
            token
        });
    }
}
