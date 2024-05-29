using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.Service.Interfaces;

namespace TeraAuthApi.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class AdminController : BaseController
{
    private readonly IAdminService _adminService;
    private readonly IUserService _userService;
    private readonly ILogger<AdminController> _logger;

    public AdminController(IAdminService adminService,
        IUserService userService,
        ILogger<AdminController> logger)
    {
        _adminService = adminService;
        _userService = userService;
        _logger = logger;
    }
    

    [HttpGet("users")]
    public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
    {
        var result = await _adminService.GetAllUsersAsync(cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }
    
    [HttpGet("users/{id}")]
    public async Task<IActionResult> GetUserProfile(Guid id, 
        CancellationToken cancellationToken = default)
    {
        var result = await _userService.GetUserProfileAsync(id, cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }

    [HttpPut("users/{id}")]
    public async Task<IActionResult> UpdateUserProfile(Guid id, [FromBody] UpdateUserInputDto updateUserInputDto,
        CancellationToken cancellationToken = default)
    {
        var result = await _adminService.UpdateUserProfileAsync(id, updateUserInputDto, GetUserId(), cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }
    [HttpDelete("users/{id}")]
    public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
    {
        var result = await _adminService.DeleteUserAsync(id, GetUserId(), cancellationToken: cancellationToken);
        return result.Success ? Ok(result) : NotFound(result);
    }
}