using Microsoft.AspNetCore.Mvc;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.DTOs.Response;
using TeraAuthApi.Application.Service.Interfaces;

namespace TeraAuthApi.Api.Controllers;


[ApiController]
[Route("api/[controller]")]

public class AuthController : BaseController
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }
    
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserInputDto registerUserInputDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.RegisterUserAsync(registerUserInputDto, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginInputDto loginInputDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.LoginAsync(loginInputDto, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : Unauthorized(result);
    }
    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenInputDto refreshTokenInputDto,
        CancellationToken cancellationToken = default)
    {
        var result = await _authService.RefreshTokenAsync(refreshTokenInputDto, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : Unauthorized(result);
    }
}