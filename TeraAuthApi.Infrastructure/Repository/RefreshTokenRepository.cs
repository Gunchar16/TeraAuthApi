using Microsoft.EntityFrameworkCore;
using TeraAuthApi.Application.Repository.Interfaces;
using TeraAuthApi.Domain.Entities;
using TeraAuthApi.Infrastructure.DataContext;

namespace TeraAuthApi.Infrastructure.Repository;

public class RefreshTokenRepository(TeraDbContext context) : BaseRepository(context), IRefreshTokenRepository
{
    public async Task<bool> SaveRefreshTokenAsync(RefreshToken refreshTokenEntity,
        CancellationToken cancellationToken = default)
    {
        _ = await _context.RefreshTokens.AddAsync(refreshTokenEntity, cancellationToken: cancellationToken);
        return await _context.SaveChangesAsync(cancellationToken: cancellationToken) > 0;
    }

    public Task<RefreshToken> GetRefreshTokenByValueAsync(string tokenValue,
        CancellationToken cancellationToken = default)
        => _context.RefreshTokens.AsNoTracking()
            .FirstOrDefaultAsync(t => t.Token == tokenValue, cancellationToken: cancellationToken);
    
    public Task UpdateAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        context.RefreshTokens.Update(refreshToken);
        return Task.FromResult(refreshToken);
    }

}