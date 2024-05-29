using Microsoft.EntityFrameworkCore;
using TeraAuthApi.Application.Repository.Interfaces;
using TeraAuthApi.Domain.Entities;
using TeraAuthApi.Infrastructure.DataContext;

namespace TeraAuthApi.Infrastructure.Repository;

public class RoleRepository(TeraDbContext context) : BaseRepository(context), IRoleRepository
{
    public Task<Role> GetByIdAsync(Guid id, 
        CancellationToken cancellationToken = default)
        => context.Roles
            .AsNoTracking()
            .SingleOrDefaultAsync(u => u.Id == id, cancellationToken: cancellationToken);

    public Task<List<Role>> GetAllAsync(
        CancellationToken cancellationToken = default)
        => context.Roles
            .AsNoTracking().ToListAsync(cancellationToken: cancellationToken);
}