using System.Data;
using Dapper;
using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Constants;
using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Infrastructure.Repositories;

public sealed class EmployeeRepository(IDbConnectionFactory connectionFactory) : IEmployeeRepository
{
    public async Task<IReadOnlyList<Employee>> GetByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.EmployeeGet,
            new { Action = EnumLookup.DbAction.List, OrganizationId = organizationId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<Employee>(command);
        return rows.AsList();
    }

    public async Task<IReadOnlyList<Employee>> GetAllActiveAsync(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.EmployeeGet,
            new { Action = EnumLookup.DbAction.All },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<Employee>(command);
        return rows.AsList();
    }

    public async Task<IReadOnlyList<Employee>> GetWithoutLoginAsync(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.EmployeeGet,
            new { Action = EnumLookup.DbAction.WithoutLogin },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<Employee>(command);
        return rows.AsList();
    }

    public async Task<Employee?> GetByIdAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.EmployeeGet,
            new { Action = EnumLookup.DbAction.Single, EmployeeId = employeeId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<Employee>(command);
    }

    public async Task<Employee?> GetByIdAndOrganizationIdAsync(int employeeId, int organizationId, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.EmployeeGet,
            new { Action = EnumLookup.DbAction.ByOrgAndId, EmployeeId = employeeId, OrganizationId = organizationId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<Employee>(command);
    }

    public async Task<DbOperationResult> ManageAsync(string action, Employee employee, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var parameters = new
        {
            Action = action,
            employee.EmployeeId,
            employee.OrganizationId,
            employee.EmployeeCode,
            employee.FullName,
            employee.Email,
            employee.MobileNumber,
            employee.Department,
            employee.Designation
        };
        var command = new CommandDefinition(
            StoredProcedures.EmployeeManage,
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<DbOperationResult>(command);
        return result ?? new DbOperationResult { Success = false, Message = "An unexpected database error occurred.", ResultId = 0 };
    }
}
