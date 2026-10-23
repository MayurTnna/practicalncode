using System.Data;
using Dapper;
using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Constants;
using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Infrastructure.Repositories;

public sealed class UserRepository(IDbConnectionFactory connectionFactory) : IUserRepository
{
    public async Task<User?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.UserGet,
            new { Action = EnumLookup.DbAction.ByUsername, Username = username },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<User>(command);
    }

    public async Task<User?> GetByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.UserGet,
            new { Action = EnumLookup.DbAction.Single, UserId = userId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<User>(command);
    }

    public async Task<IReadOnlyList<User>> GetAllUsersAsync(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.UserGet,
            new { Action = EnumLookup.DbAction.List },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<User>(command);
        return rows.AsList();
    }

    public async Task<bool> UsernameExistsAsync(string username, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.UserGet,
            new { Action = EnumLookup.DbAction.Exists, Username = username },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }

    public async Task<bool> EmployeeHasUserAsync(int employeeId, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.UserGet,
            new { Action = EnumLookup.DbAction.WithoutLogin, EmployeeId = employeeId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var count = await connection.ExecuteScalarAsync<int>(command);
        return count > 0;
    }

    public async Task<DbOperationResult> ManageAsync(string action, User user, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var parameters = new
        {
            Action = action,
            user.UserId,
            user.Username,
            user.PasswordHash,
            user.PasswordSalt,
            user.RoleId,
            user.OrganizationId,
            user.EmployeeId,
            user.IsActive
        };
        var command = new CommandDefinition(
            StoredProcedures.UserManage,
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<DbOperationResult>(command);
        return result ?? new DbOperationResult { Success = false, Message = "An unexpected database error occurred.", ResultId = 0 };
    }
}
