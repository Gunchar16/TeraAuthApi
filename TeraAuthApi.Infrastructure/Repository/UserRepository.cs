using Microsoft.EntityFrameworkCore;
using TeraAuthApi.Application.Repository.Interfaces;
using TeraAuthApi.Domain.Entities;
using TeraAuthApi.Infrastructure.DataContext;

namespace TeraAuthApi.Infrastructure.Repository;

public class UserRepository(TeraDbContext context) : IUserRepository
{
    public Task<User> GetByIdWithRoleAsync(Guid id, 
        CancellationToken cancellationToken = default)
        => context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    public Task<User> GetByIdWithRoleWithTrackingAsync(Guid id, 
        CancellationToken cancellationToken = default)
        => context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);
    
    public Task<User> GetByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
        => context.Users
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

    public Task<User> GetByUsernameAsync(string username, 
        CancellationToken cancellationToken = default)
        => context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Username == username, cancellationToken: cancellationToken);

    public Task<User> GetByEmailAsync(string email,
        CancellationToken cancellationToken = default)
        => context.Users.AsNoTracking()
            .SingleOrDefaultAsync(u => u.Email == email, cancellationToken: cancellationToken);

    public Task<User> GetByRefreshTokenAsync(string refreshToken,
        CancellationToken cancellationToken = default)
        => context.Users.AsNoTracking()
            .Include(u => u.RefreshTokens)
            .SingleOrDefaultAsync(u => u.RefreshTokens.Any(t => t.Token == refreshToken),
                cancellationToken: cancellationToken);

    public Task<List<User>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => context.Users
            .Include(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .AsNoTracking().ToListAsync(cancellationToken: cancellationToken);

    public async Task<User> AddAsync(User user, 
        CancellationToken cancellationToken = default)
    {
        var userRole =
            await context.Roles.FirstOrDefaultAsync(r => r.Name == "User", cancellationToken: cancellationToken);
        if (userRole is not null)
        {
            user.UserRoles = new List<UserRole>
            {
                new UserRole { User = user, Role = userRole }
            };
        }

        _ = await context.Users.AddAsync(user, cancellationToken: cancellationToken);
        return user;
    }

    public Task<User> UpdateAsync(User user, 
        CancellationToken cancellationToken = default)
    {
        context.Users.Update(user);
        return Task.FromResult(user);
    }

    public Task DeleteAsync(User user, 
        CancellationToken cancellationToken = default) 
        => Task.FromResult(context.Users.Remove(user));

    public Task<bool> IsEmailOrUsernameTaken(Guid userId, string userName, string email,
        CancellationToken cancellationToken = default)
    {
        return context.Users.AsNoTracking().AnyAsync(
            u => (u.Username == userName || u.Email == email) && u.Id != userId,
            cancellationToken: cancellationToken);
    }
}