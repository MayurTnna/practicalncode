using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Models;

namespace PracticalNCode.Application.Services;

public sealed class EmployeeService(IEmployeeRepository employeeRepository) : IEmployeeService
{
    public async Task<EmployeeProfileViewModel?> GetEmployeeProfileAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        var employee = await employeeRepository.GetByIdAsync(employeeId, cancellationToken);
        if (employee is null || employee.IsDeleted)
        {
            return null;
        }

        return new EmployeeProfileViewModel
        {
            EmployeeId = employee.EmployeeId,
            EmployeeCode = employee.EmployeeCode,
            FullName = employee.FullName,
            Email = employee.Email,
            MobileNumber = employee.MobileNumber,
            Department = employee.Department,
            Designation = employee.Designation,
            OrganizationName = employee.OrganizationName ?? string.Empty,
            CreatedAt = employee.CreatedAt,
            UpdatedAt = employee.UpdatedAt
        };
    }
}

