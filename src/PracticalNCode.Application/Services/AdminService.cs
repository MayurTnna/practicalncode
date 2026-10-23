using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Common;
using PracticalNCode.Application.Constants;
using PracticalNCode.Application.Models;
using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Services;

public sealed class AdminService(
    IOrganizationRepository organizationRepository,
    IEmployeeRepository employeeRepository,
    IUserRepository userRepository) : IAdminService
{
    public async Task<IReadOnlyList<OrganizationItemViewModel>> GetAllOrganizationsAsync(CancellationToken cancellationToken = default)
    {
        var organizations = await organizationRepository.GetAllAsync(cancellationToken);
        return organizations.Select(o => new OrganizationItemViewModel
        {
            OrganizationId = o.OrganizationId,
            OrganizationName = o.OrganizationName,
            ContactEmail = o.ContactEmail,
            ContactPhone = o.ContactPhone,
            IsActive = o.IsActive,
            CreatedAt = o.CreatedAt
        }).ToArray();
    }

    public async Task<(bool Success, string? ErrorMessage, int OrganizationId)> CreateOrganizationWithLoginAsync(CreateOrganizationViewModel model, CancellationToken cancellationToken = default)
    {
        var organization = new Organization
        {
            OrganizationName = model.OrganizationName.Trim(),
            ContactEmail = string.IsNullOrWhiteSpace(model.ContactEmail) ? null : model.ContactEmail.Trim(),
            ContactPhone = string.IsNullOrWhiteSpace(model.ContactPhone) ? null : model.ContactPhone.Trim(),
            IsActive = true
        };

        var orgResult = await organizationRepository.ManageAsync(EnumLookup.DbAction.Insert, organization, cancellationToken);
        if (!orgResult.Success)
        {
            return (false, orgResult.Message, 0);
        }

        var (hash, salt) = PasswordHelper.HashPassword(model.Password);
        var orgUser = new User
        {
            Username = model.Username.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            RoleId = (int)EnumLookup.UserRole.Organization,
            OrganizationId = orgResult.ResultId,
            EmployeeId = null,
            IsActive = true
        };

        var userResult = await userRepository.ManageAsync(EnumLookup.DbAction.Insert, orgUser, cancellationToken);
        if (!userResult.Success)
        {
            return (false, userResult.Message, 0);
        }

        return (true, null, orgResult.ResultId);
    }

    public async Task<IReadOnlyList<EmployeeItemViewModel>> GetAllEmployeesAsync(CancellationToken cancellationToken = default)
    {
        var employees = await employeeRepository.GetAllActiveAsync(cancellationToken);
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

    public async Task<IReadOnlyList<EmployeeItemViewModel>> GetEmployeesWithoutLoginAsync(CancellationToken cancellationToken = default)
    {
        var employees = await employeeRepository.GetWithoutLoginAsync(cancellationToken);
        return employees.Select(emp => new EmployeeItemViewModel
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
            HasLoginAccount = false,
            CreatedAt = emp.CreatedAt,
            UpdatedAt = emp.UpdatedAt
        }).ToArray();
    }

    public async Task<CreateEmployeeLoginViewModel?> GetEmployeeForLoginCreationAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        var emp = await employeeRepository.GetByIdAsync(employeeId, cancellationToken);
        if (emp is null || emp.IsDeleted)
        {
            return null;
        }

        var hasLogin = await userRepository.EmployeeHasUserAsync(employeeId, cancellationToken);
        if (hasLogin)
        {
            return null;
        }

        return new CreateEmployeeLoginViewModel
        {
            EmployeeId = emp.EmployeeId,
            EmployeeCode = emp.EmployeeCode,
            EmployeeName = emp.FullName,
            OrganizationName = emp.OrganizationName,
            EmployeeEmail = emp.Email,
            Username = emp.Email
        };
    }

    public async Task<(bool Success, string? ErrorMessage, int UserId)> CreateEmployeeLoginAsync(CreateEmployeeLoginViewModel model, CancellationToken cancellationToken = default)
    {
        var emp = await employeeRepository.GetByIdAsync(model.EmployeeId, cancellationToken);
        if (emp is null || emp.IsDeleted)
        {
            return (false, "The selected employee record does not exist or has been deleted.", 0);
        }

        var (hash, salt) = PasswordHelper.HashPassword(model.Password);
        var empUser = new User
        {
            Username = model.Username.Trim(),
            PasswordHash = hash,
            PasswordSalt = salt,
            RoleId = (int)EnumLookup.UserRole.Employee,
            OrganizationId = emp.OrganizationId,
            EmployeeId = emp.EmployeeId,
            IsActive = true
        };

        var userResult = await userRepository.ManageAsync(EnumLookup.DbAction.Insert, empUser, cancellationToken);
        if (!userResult.Success)
        {
            return (false, userResult.Message, 0);
        }

        return (true, null, userResult.ResultId);
    }

    public async Task<IReadOnlyList<UserItemViewModel>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        var users = await userRepository.GetAllUsersAsync(cancellationToken);
        return users.Select(u => new UserItemViewModel
        {
            UserId = u.UserId,
            Username = u.Username,
            RoleName = u.RoleName ?? string.Empty,
            OrganizationName = u.OrganizationName,
            EmployeeName = u.EmployeeName,
            EmployeeCode = u.EmployeeCode,
            IsActive = u.IsActive,
            CreatedAt = u.CreatedAt
        }).ToArray();
    }
}
