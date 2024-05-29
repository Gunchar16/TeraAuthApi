using TeraAuthApi.Application.Repository.Interfaces;

namespace TeraAuthApi.Application.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}