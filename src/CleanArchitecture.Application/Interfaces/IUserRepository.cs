using CleanArchitecture.Domain.Entities;

namespace CleanArchitecture.Application.Interfaces;

public interface IUserRepository
{
    Task<Guid> AddAsync(User user, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> GetAllAsync(CancellationToken cancellationToken = default);
}