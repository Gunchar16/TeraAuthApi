using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace TeraAuthApi.Api.Controllers;

public class BaseController : ControllerBase
{
    protected Guid GetUserId()
    {
        var httpContext = HttpContext;
        var idClaim = httpContext.User.FindFirst(ClaimTypes.NameIdentifier);
        Guid.TryParse(idClaim?.Value, out var id);
        return id;
    }

    protected string GetUserRole()
    {
        var roleClaim = HttpContext.User.FindFirst(ClaimTypes.Role);
        var role = roleClaim?.Value ?? "User";
        return role;
    }
    
}