using PracticalNCode.Application.Abstractions.Repositories;
using PracticalNCode.Application.Abstractions.Services;
using PracticalNCode.Application.Models;
namespace PracticalNCode.Application.Services;
public sealed class AiModelService(IAiModelRepository repository) : IAiModelService
{
    public async Task<IReadOnlyList<AiModelListItem>> GetActiveAsync(CancellationToken cancellationToken = default)
    {
        var models = await repository.GetActiveAsync(cancellationToken);
        return models.Select(x => new AiModelListItem(x.AiModelId, x.Name, x.Provider, x.Description, x.WebsiteUrl)).ToArray();
    }
}
