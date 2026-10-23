namespace PracticalNCode.Application.Models;

public sealed class OrganizationItemViewModel
{
    public int OrganizationId { get; set; }
    public string OrganizationName { get; set; } = string.Empty;
    public string? ContactEmail { get; set; }
    public string? ContactPhone { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

