using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.Wrapper;

namespace TeraAuthApi.Application.Service;

public interface IRoleService
{
    Task<ApiServiceResponse<List<RoleDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId,
        CancellationToken cancellationToken = default);
}