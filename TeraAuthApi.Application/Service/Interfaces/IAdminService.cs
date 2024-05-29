using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.Wrapper;

namespace TeraAuthApi.Application.Service.Interfaces;

public interface IAdminService
{
    Task<ApiServiceResponse<List<UserDto>>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<UserDto>> UpdateUserProfileAsync(Guid id, UpdateUserInputDto updateUserInputDto, Guid requesterUserId,
        CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<bool>> DeleteUserAsync(Guid id, Guid requesterUserId,
        CancellationToken cancellationToken = default);
}