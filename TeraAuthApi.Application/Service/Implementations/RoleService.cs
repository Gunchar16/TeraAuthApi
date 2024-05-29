using AutoMapper;
using Microsoft.Extensions.Logging;
using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.UnitOfWork;
using TeraAuthApi.Application.Wrapper;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Service.Implementations;

public class RoleService : IRoleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RoleService> _logger;
    private readonly IMapper _mapper;

    public RoleService(IUnitOfWork unitOfWork, ILogger<RoleService> logger, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<ApiServiceResponse<List<RoleDto>>> GetAllRolesAsync(CancellationToken cancellationToken = default)
    {
        var roles = await _unitOfWork.RoleRepository.GetAllAsync(cancellationToken: cancellationToken);
        var roleDtos = _mapper.Map<List<RoleDto>>(roles);
        return ApiServiceResponse<List<RoleDto>>.SuccessResponse(roleDtos);
    }

    public async Task<ApiServiceResponse<bool>> AssignRoleToUserAsync(Guid userId, Guid roleId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithRoleWithTrackingAsync(userId, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<bool>.FailureResponse("User not found");

        var role = await _unitOfWork.RoleRepository.GetByIdAsync(roleId, cancellationToken: cancellationToken);
        if (role is null)
            return ApiServiceResponse<bool>.FailureResponse("Role not found");

        var existingUserRole = user.UserRoles?.FirstOrDefault(ur => ur.UserId == userId);
    
        if (existingUserRole is not null)
            existingUserRole.RoleId = role.Id;
        else
        {
            var userRole = new UserRole { UserId = userId, RoleId = role.Id };
            user.UserRoles?.Add(userRole);
        }

        var wasSuccess = await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ApiServiceResponse<bool>.SuccessResponse(true, "Role assignment updated successfully");
    }

}