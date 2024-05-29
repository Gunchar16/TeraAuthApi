using TeraAuthApi.Application.Repository.Interfaces;

namespace TeraAuthApi.Application.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository UserRepository { get; }
    IRoleRepository RoleRepository { get; }
    IRefreshTokenRepository RefreshTokenRepository { get; }
    Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
}