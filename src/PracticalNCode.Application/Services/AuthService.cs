using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Common;
using PracticalNCode.Application.Constants;
using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Services;

public sealed class AuthService(
    IUserRepository userRepository,
    IEmployeeRepository employeeRepository,
    IOrganizationRepository organizationRepository) : IAuthService
{
    public async Task<(bool Success, string? ErrorMessage, User? User)> ValidateUserAsync(string username, string password, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            return (false, "Username and password are required.", null);
        }

        var user = await userRepository.GetByUsernameAsync(username.Trim(), cancellationToken);
        if (user is null)
        {
            return (false, "Invalid username or password.", null);
        }

        var isPasswordValid = PasswordHelper.VerifyPassword(password, user.PasswordHash, user.PasswordSalt);
        if (!isPasswordValid)
        {
            return (false, "Invalid username or password.", null);
        }

        if (!user.IsActive)
        {
            return (false, "This account is inactive. Please contact your administrator.", null);
        }

        // If employee role, ensure linked employee record is active and not deleted
        if (user.RoleId == (int)EnumLookup.UserRole.Employee)
        {
            if (!user.EmployeeId.HasValue)
            {
                return (false, "This employee login is not linked to a valid employee record.", null);
            }

            var employee = await employeeRepository.GetByIdAsync(user.EmployeeId.Value, cancellationToken);
            if (employee is null || employee.IsDeleted)
            {
                return (false, "Access denied. The employee record linked to this account has been deleted.", null);
            }
        }

        // If organization role, ensure organization is active
        if (user.RoleId == (int)EnumLookup.UserRole.Organization)
        {
            if (!user.OrganizationId.HasValue)
            {
                return (false, "This organization login is not linked to a valid organization record.", null);
            }

            var organization = await organizationRepository.GetByIdAsync(user.OrganizationId.Value, cancellationToken);
            if (organization is null || !organization.IsActive)
            {
                return (false, "This organization account is currently inactive.", null);
            }
        }

        return (true, null, user);
    }

    public async Task EnsureInitialAdminExistsAsync(CancellationToken cancellationToken = default)
    {
        var adminExists = await userRepository.UsernameExistsAsync("admin", cancellationToken);
        if (!adminExists)
        {
            var (hash, salt) = PasswordHelper.HashPassword("Admin@123");
            var adminUser = new User
            {
                Username = "admin",
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = (int)EnumLookup.UserRole.Admin,
                OrganizationId = null,
                EmployeeId = null,
                IsActive = true
            };
            await userRepository.ManageAsync(EnumLookup.DbAction.Insert, adminUser, cancellationToken);
        }
    }
}
