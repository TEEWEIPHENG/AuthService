using AuthService.Application.Interfaces.Services;
using ApplicationModels = AuthService.Application.Models;
using APIModels = AuthService.API.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AuthService.API.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// Register a new user account
    /// </summary>
    /// <param name="request">RegisterRequest(Firstname, Lastname, Username, Email, Password)</param>
    /// <returns>AuthResult</returns>
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register([FromBody] APIModels.RegisterRequest request)
    {
        _logger.LogInformation("Register attempt for {Email}, CorrelationId={CorrelationId}",
            request.Email,
            HttpContext.Items["X-Correlation-ID"]);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var appRequest = new ApplicationModels.RegisterRequest(
               request.Username,
               request.Email,
               request.Password
           );
        var result = await _authService.RegisterAsync(appRequest);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Register failed for {Email}: {Errors}", request.Email, string.Join(", ", result.Errors));
            return BadRequest(result.Errors);
        }

        _logger.LogInformation("User registered successfully: {Email}", request.Email);
        return Ok(result);
    }

    //[HttpPost("login")]
    //[AllowAnonymous]
    //public async Task<ActionResult<AuthResult>> Login(LoginRequest request)
    //{
    //    var result = await _authService.LoginAsync(new LoginRequest(request.Username, request.Password));
    //    return Ok(result);
    //}

    //[HttpPost("logout")]
    //[Authorize]
    //public async Task<IActionResult> Logout()
    //{
    //    var userId = User.FindFirst("sub")?.Value!;
    //    await _authService.LogoutAsync(userId);
    //    return NoContent();
    //}


    //[HttpPost("refresh-token")]
    //[AllowAnonymous]
    //public async Task<ActionResult<AuthResult>> RefreshToken(RefreshTokenRequest request)
    //{
    //    var result = await _authService.RefreshTokenAsync(new RefreshTokenRequest());
    //    return Ok(result);
    //}

    //[HttpGet("profile")]
    //[Authorize]
    //public async Task<ActionResult<UserProfileDto>> GetProfile()
    //{
    //    var userId = User.FindFirst("sub")?.Value!;
    //    var profile = await _authService.GetProfileAsync(userId);
    //    return Ok(profile);
    //}
    //[HttpPost("change-password")]
    //[Authorize]
    //public async Task<IActionResult> ChangePassword()
    //{
    //    var userId = User.FindFirst("sub")?.Value!;
    //    await _authService.ChangePasswordAsync(userId);
    //    return NoContent();
    //}
}
