namespace PracticalNCode.Application.Models;

public sealed class UserItemViewModel
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? OrganizationName { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeCode { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
}

