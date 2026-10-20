using Dapper;
using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Domain.Entities;
namespace PracticalNCode.Infrastructure.Repositories;
public sealed class AiModelRepository(IDbConnectionFactory connectionFactory) : IAiModelRepository
{
    private const string GetActiveModelsSql = """
        SELECT AiModelId, Name, Provider, Description, WebsiteUrl, IsActive, CreatedAt
        FROM dbo.AiModels WHERE IsActive = @IsActive ORDER BY Name;
        """;
    public async Task<IReadOnlyList<AiModel>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        using var connection = connectionFactory.CreateConnection();
        var command = new CommandDefinition(GetActiveModelsSql, new { IsActive = true }, cancellationToken: cancellationToken);
        var rows = await connection.QueryAsync<AiModel>(command);
        return rows.AsList();
    }
}
