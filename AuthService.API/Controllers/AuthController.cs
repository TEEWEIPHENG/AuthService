using AuthService.Application.Interfaces.Services;
using AuthService.Application.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.API.Controllers;

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
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterRequest request)
    {
        var result = await _authService.RegisterAsync(new RegisterRequest(request.Username, request.Email, request.Password));
        return Ok(result);
    }

    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResult>> Login(LoginRequest request)
    {
        var result = await _authService.LoginAsync(new LoginRequest(request.Username, request.Password));
        return Ok(result);
    }

    [HttpPost("logout")]
    [Authorize]
    public async Task<IActionResult> Logout()
    {
        var userId = User.FindFirst("sub")?.Value!;
        await _authService.LogoutAsync(userId);
        return NoContent();
    }


    [HttpPost("refresh-token")]
    [AllowAnonymous]
    public async Task<ActionResult<AuthResult>> RefreshToken(RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(new RefreshTokenRequest());
        return Ok(result);
    }

    [HttpGet("profile")]
    [Authorize]
    public async Task<ActionResult<UserProfileDto>> GetProfile()
    {
        var userId = User.FindFirst("sub")?.Value!;
        var profile = await _authService.GetProfileAsync(userId);
        return Ok(profile);
    }
    [HttpPost("change-password")]
    [Authorize]
    public async Task<IActionResult> ChangePassword()
    {
        var userId = User.FindFirst("sub")?.Value!;
        await _authService.ChangePasswordAsync(userId);
        return NoContent();
    }
}
