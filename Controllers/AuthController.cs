using LauncherHero.Starter.Models;
using LauncherHero.Starter.Services;
using Microsoft.AspNetCore.Mvc;

namespace LauncherHero.Starter.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthService _authService;

    public AuthController(AuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(request);
        return result is null ? Conflict(new { message = "Email already exists." }) : Created(string.Empty, result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(request);
        return result is null ? Unauthorized(new { message = "Invalid credentials." }) : Ok(result);
    }
}
