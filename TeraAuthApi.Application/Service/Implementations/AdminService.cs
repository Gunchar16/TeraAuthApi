using AutoMapper;
using Microsoft.Extensions.Logging;
using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.Service.Interfaces;
using TeraAuthApi.Application.UnitOfWork;
using TeraAuthApi.Application.Wrapper;
using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Service.Implementations;

public class AdminService : IAdminService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<AdminService> _logger;
    private readonly IMapper _mapper;

    public AdminService(
        IUnitOfWork unitOfWork, 
        ILogger<AdminService> logger, 
        IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
        _mapper = mapper;
    }
    
    public async Task<ApiServiceResponse<List<UserDto>>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(cancellationToken: cancellationToken);
        var mappedUsers = _mapper.Map<List<UserDto>>(users);
        return ApiServiceResponse<List<UserDto>>.SuccessResponse(mappedUsers);
    }
    

    public async Task<ApiServiceResponse<UserDto>> UpdateUserProfileAsync(Guid id, UpdateUserInputDto updateUserInputDto, Guid requesterUserId,
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithRoleAsync(id, cancellationToken: cancellationToken);
        if (user is null)
            return ApiServiceResponse<UserDto>.FailureResponse("User not found");

        if (IsAdmin(user) && id != requesterUserId)
            return ApiServiceResponse<UserDto>.FailureResponse("Cannot update an admin user");
        
        var isEmailOrUsernameTaken = await _unitOfWork.UserRepository.IsEmailOrUsernameTaken(id, updateUserInputDto.Username, updateUserInputDto.Email,
            cancellationToken: cancellationToken);
        if (isEmailOrUsernameTaken)
            return ApiServiceResponse<UserDto>.FailureResponse("Username or email is already taken");

        user.Username = updateUserInputDto.Username;
        user.Email = updateUserInputDto.Email;
        user.UpdatedAt = DateTime.UtcNow;

        var updatedUser = await _unitOfWork.UserRepository.UpdateAsync(user, cancellationToken: cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken: cancellationToken);

        var mappedUser = _mapper.Map<UserDto>(updatedUser);
        _logger.LogInformation("User profile updated successfully. UserId: {UserId}", updatedUser.Id);
        return ApiServiceResponse<UserDto>.SuccessResponse(mappedUser, "User profile updated successfully");
    }

    
    public async Task<ApiServiceResponse<bool>> DeleteUserAsync(Guid id, Guid requesterUserId, 
        CancellationToken cancellationToken = default)
    {
        var user = await _unitOfWork.UserRepository.GetByIdWithRoleAsync(id, cancellationToken);
        if (user is null)
            return ApiServiceResponse<bool>.FailureResponse("User not found");

        if (IsAdmin(user) && id != requesterUserId)
        {
            return ApiServiceResponse<bool>.FailureResponse("Cannot delete an admin user");
        }

        await _unitOfWork.UserRepository.DeleteAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User deleted successfully. UserId: {UserId}", id);
        return ApiServiceResponse<bool>.SuccessResponse(true, "User deleted successfully");
    }

    private bool IsAdmin(User user)
    {
        return user.UserRoles is not null &&
               user.UserRoles.Any(role => role.Role.Name.Equals("Admin", StringComparison.OrdinalIgnoreCase));
    }
}