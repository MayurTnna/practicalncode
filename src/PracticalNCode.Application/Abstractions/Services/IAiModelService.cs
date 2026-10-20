using PracticalNCode.Application.Models;
namespace PracticalNCode.Application.Abstractions.Services;
public interface IAiModelService
{
    Task<IReadOnlyList<AiModelListItem>> GetActiveAsync(CancellationToken cancellationToken = default);
}
