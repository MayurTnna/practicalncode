using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Abstractions.Repositories;

public interface IEmployeeRepository
{
    Task<IReadOnlyList<Employee>> GetByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetAllActiveAsync(CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Employee>> GetWithoutLoginAsync(CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken = default);
    Task<Employee?> GetByIdAndOrganizationIdAsync(int employeeId, int organizationId, CancellationToken cancellationToken = default);
    Task<DbOperationResult> ManageAsync(string action, Employee employee, CancellationToken cancellationToken = default);
}
