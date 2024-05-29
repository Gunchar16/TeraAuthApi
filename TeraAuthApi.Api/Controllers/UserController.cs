using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.Service.Interfaces;

namespace TeraAuthApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : BaseController
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpGet("profile")]
    public async Task<IActionResult> GetUserProfile(CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserProfileAsync(GetUserId(), cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPut("profile")]
    public async Task<IActionResult> UpdateUserProfile([FromBody] UpdateUserInputDto updateUserInputDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.UpdateUserProfileAsync(GetUserId(), updateUserInputDto, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordInputDto changePasswordInputDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.ChangePasswordAsync(GetUserId(), changePasswordInputDto, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordInputDto resetPasswordInputDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.ResetPasswordAsync(resetPasswordInputDto, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}