using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Abstractions.Services;

public interface IAuthService
{
    Task<(bool Success, string? ErrorMessage, User? User)> ValidateUserAsync(string username, string password, CancellationToken cancellationToken = default);
    Task EnsureInitialAdminExistsAsync(CancellationToken cancellationToken = default);
}

