using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.Service;

namespace TeraAuthApi.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController : BaseController
{
    private readonly IRoleService _roleService;
    private readonly ILogger<RoleController> _logger;

    public RoleController(IRoleService roleService, ILogger<RoleController> logger)
    {
        _roleService = roleService;
        _logger = logger;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken = default)
    {
        var result = await _roleService.GetAllRolesAsync(cancellationToken);
        return Ok(result);
    }

    [HttpPost("assign")]
    public async Task<IActionResult> AssignRoleToUser([FromBody] AssignRoleInputDto assignRoleInputDto, 
        CancellationToken cancellationToken = default)
    {
        var result = await _roleService.AssignRoleToUserAsync(assignRoleInputDto.UserId, assignRoleInputDto.RoleId, cancellationToken);
        return result.Success ? Ok(result) : BadRequest(result);
    }
}