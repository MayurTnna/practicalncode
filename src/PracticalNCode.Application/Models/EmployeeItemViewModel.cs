namespace PracticalNCode.Application.Models;

public sealed class EmployeeItemViewModel
{
    public int EmployeeId { get; set; }
    public int OrganizationId { get; set; }
    public string EmployeeCode { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Designation { get; set; } = string.Empty;
    public string? OrganizationName { get; set; }
    public bool HasLoginAccount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

