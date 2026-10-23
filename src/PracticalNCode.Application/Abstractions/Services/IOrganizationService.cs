using PracticalNCode.Application.Models;

namespace PracticalNCode.Application.Abstractions.Services;

public interface IOrganizationService
{
    Task<IReadOnlyList<EmployeeItemViewModel>> GetEmployeesByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<(bool Success, string? ErrorMessage, int EmployeeId)> CreateEmployeeAsync(CreateEmployeeViewModel model, CancellationToken cancellationToken = default);
    Task<EditEmployeeViewModel?> GetEmployeeForEditAsync(int employeeId, int organizationId, CancellationToken cancellationToken = default);
    Task<(bool Success, string? ErrorMessage)> UpdateEmployeeAsync(EditEmployeeViewModel model, CancellationToken cancellationToken = default);
    Task<(bool Success, string? ErrorMessage)> DeleteEmployeeAsync(int employeeId, int organizationId, CancellationToken cancellationToken = default);
}

