using TeraAuthApi.Application.DTOs;
using TeraAuthApi.Application.DTOs.Request;
using TeraAuthApi.Application.DTOs.Response;
using TeraAuthApi.Application.Wrapper;

namespace TeraAuthApi.Application.Service.Interfaces;

public interface IAuthService
{
    Task<ApiServiceResponse<AuthResponseDto>> RegisterUserAsync(RegisterUserInputDto registerUserInputDto,
        CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<AuthResponseDto>> LoginAsync(LoginInputDto loginInputDto,
        CancellationToken cancellationToken = default);
    Task<ApiServiceResponse<AuthResponseDto>> RefreshTokenAsync(RefreshTokenInputDto refreshTokenInputDto,
        CancellationToken cancellationToken = default);

}