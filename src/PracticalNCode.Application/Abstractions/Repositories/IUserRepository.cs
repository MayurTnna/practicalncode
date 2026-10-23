using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Abstractions.Repositories;

public interface IUserRepository
{
    Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<User>> GetAllUsersAsync(CancellationToken cancellationToken = default);
    Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> EmployeeHasUserAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<DbOperationResult> ManageAsync(string action, User user, CancellationToken cancellationToken = default);
}
