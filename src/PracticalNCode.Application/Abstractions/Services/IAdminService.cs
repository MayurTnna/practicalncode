using PracticalNCode.Application.Models;

namespace PracticalNCode.Application.Abstractions.Services;

public interface IAdminService
{
    Task<IReadOnlyList<OrganizationItemViewModel>> GetAllOrganizationsAsync(CancellationToken cancellationToken = default);
    Task<(bool Success, string? ErrorMessage, int OrganizationId)> CreateOrganizationWithLoginAsync(CreateOrganizationViewModel model, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EmployeeItemViewModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<EmployeeItemViewModel>> GetEmployeesWithoutLoginAsync(CancellationToken cancellationToken = default);
    Task<CreateEmployeeLoginViewModel?> GetEmployeeForLoginCreationAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<(bool Success, string? ErrorMessage, int UserId)> CreateEmployeeLoginAsync(CreateEmployeeLoginViewModel model, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<UserItemViewModel>> GetAllUsersAsync(CancellationToken cancellationToken = default);
}

