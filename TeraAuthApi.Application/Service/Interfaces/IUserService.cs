using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.Wrapper;

namespace TeraAuthApi.Application.Service.Interfaces;

public interface IUserService
{
    Task<ApiServiceResponse<UserDto>> GetUserProfileAsync(Guid userId,
        CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<UserDto>> UpdateUserProfileAsync(Guid userId, UpdateUserInputDto updateUserInputDto,
        CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<bool>> ChangePasswordAsync(Guid userId, ChangePasswordInputDto changePasswordInputDto,
        CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<bool>> ResetPasswordAsync(ResetPasswordInputDto resetPasswordInputDto,
        CancellationToken cancellationToken = default);
}