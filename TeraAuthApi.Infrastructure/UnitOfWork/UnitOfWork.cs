using TeraAuthApi.Application.Repository.Interfaces;
using TeraAuthApi.Application.UnitOfWork;
using TeraAuthApi.Infrastructure.DataContext;
using TeraAuthApi.Infrastructure.Repository;

namespace TeraAuthApi.Infrastructure.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    private readonly TeraDbContext _context;
    private IUserRepository _userRepository;
    private IRoleRepository _roleRepository;
    private IRefreshTokenRepository _refreshTokenRepository;

    public UnitOfWork(TeraDbContext context)
    {
        _context = context;
    }

    public IUserRepository UserRepository
    {
        get
        {
            if (_userRepository is null)
                _userRepository = new UserRepository(_context);
            return _userRepository;
        }
    }
    public IRoleRepository RoleRepository
    {
        get
        {
            if (_roleRepository is null)
                _roleRepository = new RoleRepository(_context);
            return _roleRepository;
        }
    }
    
    public IRefreshTokenRepository RefreshTokenRepository
    {
        get
        {
            if (_refreshTokenRepository is null)
                _refreshTokenRepository = new RefreshTokenRepository(_context);
            return _refreshTokenRepository;
        }
    }

    public async Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default) 
        => await _context.SaveChangesAsync(cancellationToken: cancellationToken) > 0;
    
}