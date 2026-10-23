namespace PracticalNCode.Domain.Entities;

public sealed class Organization
{
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; }
}

