using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Repository.Interfaces;

public interface IRefreshTokenRepository
{
    Task<bool> SaveRefreshTokenAsync(RefreshToken refreshTokenEntity,
        CancellationToken cancellationToken = default);

    Task<RefreshToken> GetRefreshTokenByValueAsync(string tokenValue,
        CancellationToken cancellationToken = default);

    Task UpdateAsync(RefreshToken refreshToken, 
        CancellationToken cancellationToken = default);
}