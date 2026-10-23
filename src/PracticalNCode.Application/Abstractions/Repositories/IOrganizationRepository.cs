using PracticalNCode.Domain.Entities;

namespace PracticalNCode.Application.Abstractions.Repositories;

public interface IOrganizationRepository
{
    Task<IReadOnlyList<Organization>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Organization?> GetByIdAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<DbOperationResult> ManageAsync(string action, Organization organization, CancellationToken cancellationToken = default);
}
