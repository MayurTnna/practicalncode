using PracticalNCode.Application.Models;

namespace PracticalNCode.Application.Abstractions.Services;

public interface IEmployeeService
{
    Task<EmployeeProfileViewModel?> GetEmployeeProfileAsync(int employeeId, CancellationToken cancellationToken = default);
}

