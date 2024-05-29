using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Repository.Interfaces;

public interface IRoleRepository
{
    Task<Role> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<List<Role>> GetAllAsync(
        CancellationToken cancellationToken = default);
}