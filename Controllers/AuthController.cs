using Concrete.Api.DTOs.Auth;
using Concrete.Api.Services.Auth;
using Microsoft.AspNetCore.Mvc;

namespace Concrete.Api.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        await _authService.RegisterAsync(request);
        return Created("", new RegisterResponse());
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    [HttpPost("2fa/verify")]
    public async Task<IActionResult> Verify2FA(VerifyTwoFactorRequest request)
    {
        var tokens = await _authService.VerifyTwoFactorAsync(request);
        return Ok(tokens);
    }

    [HttpPost("2fa/recovery")]
    public async Task<IActionResult> Recovery(RecoveryCodeRequest request)
    {
        var tokens = await _authService.RecoveryAsync(request);
        return Ok(tokens);
    }
}
