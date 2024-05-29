using TeraAuthApi.Domain.Entities;

namespace TeraAuthApi.Application.Repository.Interfaces;

public interface IUserRepository
{
    Task<User> GetByIdWithRoleAsync(Guid id,
        CancellationToken cancellationToken = default);

    Task<User> GetByIdWithRoleWithTrackingAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<User> GetByIdAsync(Guid id,
        CancellationToken cancellationToken = default);
    Task<User> GetByUsernameAsync(string username, 
        CancellationToken cancellationToken = default);
    Task<User> GetByEmailAsync(string email, 
        CancellationToken cancellationToken = default);
    Task<User> GetByRefreshTokenAsync(string refreshToken, 
        CancellationToken cancellationToken = default);
    Task<List<User>> GetAllAsync(
        CancellationToken cancellationToken = default);
    Task<User> AddAsync(User user, 
        CancellationToken cancellationToken = default);
    Task<User> UpdateAsync(User user, 
        CancellationToken cancellationToken = default);
    Task DeleteAsync(User user, 
        CancellationToken cancellationToken = default);

    Task<bool> IsEmailOrUsernameTaken(Guid id, string userName, string email,
        CancellationToken cancellationToken = default);
}