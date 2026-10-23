using System.Data;
using Dapper;
using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Constants;
using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Infrastructure.Repositories;

public sealed class OrganizationRepository(IDbConnectionFactory connectionFactory) : IOrganizationRepository
{
    public async Task<IReadOnlyList<Organization>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.OrganizationGet,
            new { Action = EnumLookup.DbAction.List },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<Organization>(command);
        return rows.AsList();
    }

    public async Task<Organization?> GetByIdAsync(int organizationId, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var command = new CommandDefinition(
            StoredProcedures.OrganizationGet,
            new { Action = EnumLookup.DbAction.Single, OrganizationId = organizationId },
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        return await connection.QueryFirstOrDefaultAsync<Organization>(command);
    }

    public async Task<DbOperationResult> ManageAsync(string action, Organization organization, CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.GetConnection();
        var parameters = new
        {
            Action = action,
            organization.OrganizationId,
            organization.OrganizationName,
            organization.ContactEmail,
            organization.ContactPhone,
            organization.IsActive
        };
        var command = new CommandDefinition(
            StoredProcedures.OrganizationManage,
            parameters,
            commandType: CommandType.StoredProcedure,
            cancellationToken: cancellationToken);
        var result = await connection.QueryFirstOrDefaultAsync<DbOperationResult>(command);
        return result ?? new DbOperationResult { Success = false, Message = "An unexpected database error occurred.", ResultId = 0 };
    }
}
