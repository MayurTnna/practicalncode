using PracticalNCode.Domain.Entities;
namespace PracticalNCode.Application.Abstractions.Repositories;
public interface IAiModelRepository
{
    Task<IReadOnlyList<AiModel>> GetActiveAsync(CancellationToken cancellationToken = default);
}
