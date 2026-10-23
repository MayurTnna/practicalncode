using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Constants;
using PracticalNCode.Application.Models;
using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Services;

public sealed class OrganizationService(
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository) : IOrganizationService
{
    public async Task<IReadOnlyList<EmployeeItemViewModel>> GetEmployeesByOrganizationAsync(int organizationId, CancellationToken cancellationToken = default)
    {
        var employees = await employeeRepository.GetByOrganizationIdAsync(organizationId, cancellationToken);
        var result = new List<EmployeeItemViewModel>(employees.Count);

        foreach (var emp in employees)
        {
            var hasLogin = await userRepository.EmployeeHasUserAsync(emp.EmployeeId, cancellationToken);
            result.Add(new EmployeeItemViewModel
            {
                EmployeeId = emp.EmployeeId,
                OrganizationId = emp.OrganizationId,
                EmployeeCode = emp.EmployeeCode,
                FullName = emp.FullName,
                Email = emp.Email,
                MobileNumber = emp.MobileNumber,
                Department = emp.Department,
                Designation = emp.Designation,
                OrganizationName = emp.OrganizationName,
                HasLoginAccount = hasLogin,
                CreatedAt = emp.CreatedAt,
                UpdatedAt = emp.UpdatedAt
            });
        }

        return result;
    }

    public async Task<(bool Success, string? ErrorMessage, int EmployeeId)> CreateEmployeeAsync(CreateEmployeeViewModel model, CancellationToken cancellationToken = default)
    {
        var employee = new Employee
        {
            OrganizationId = model.OrganizationId,
            EmployeeCode = model.EmployeeCode.Trim(),
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            MobileNumber = model.MobileNumber.Trim(),
            Department = model.Department.Trim(),
            Designation = model.Designation.Trim()
        };

        var result = await employeeRepository.ManageAsync(EnumLookup.DbAction.Insert, employee, cancellationToken);
        return (result.Success, result.Success ? null : result.Message, result.ResultId);
    }

    public async Task<EditEmployeeViewModel?> GetEmployeeForEditAsync(int employeeId, int organizationId, CancellationToken cancellationToken = default)
    {
        var emp = await employeeRepository.GetByIdAndOrganizationIdAsync(employeeId, organizationId, cancellationToken);
        if (emp is null)
        {
            return null;
        }

        return new EditEmployeeViewModel
        {
            EmployeeId = emp.EmployeeId,
            OrganizationId = emp.OrganizationId,
            EmployeeCode = emp.EmployeeCode,
            FullName = emp.FullName,
            Email = emp.Email,
            MobileNumber = emp.MobileNumber,
            Department = emp.Department,
            Designation = emp.Designation
        };
    }

    public async Task<(bool Success, string? ErrorMessage)> UpdateEmployeeAsync(EditEmployeeViewModel model, CancellationToken cancellationToken = default)
    {
        var employee = new Employee
        {
            EmployeeId = model.EmployeeId,
            OrganizationId = model.OrganizationId,
            EmployeeCode = model.EmployeeCode.Trim(),
            FullName = model.FullName.Trim(),
            Email = model.Email.Trim(),
            MobileNumber = model.MobileNumber.Trim(),
            Department = model.Department.Trim(),
            Designation = model.Designation.Trim()
        };

        var result = await employeeRepository.ManageAsync(EnumLookup.DbAction.Update, employee, cancellationToken);
            return (result.Success, result.Success ? null : result.Message);
    }

    public async Task<(bool Success, string? ErrorMessage)> DeleteEmployeeAsync(int employeeId, int organizationId, CancellationToken cancellationToken = default)
    {
        var employee = new Employee
        {
            EmployeeId = employeeId,
            OrganizationId = organizationId
        };

        var result = await employeeRepository.ManageAsync(EnumLookup.DbAction.Delete, employee, cancellationToken);
        return (result.Success, result.Success ? null : result.Message);
    }
}
