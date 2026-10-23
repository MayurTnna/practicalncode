namespace PracticalNCode.Domain.Entities;

public sealed class User
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public string PasswordSalt { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public int? OrganizationId { get; set; }
    public int? EmployeeId { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }

    public string? RoleName { get; set; }
    public string? OrganizationName { get; set; }
    public string? EmployeeName { get; set; }
    public string? EmployeeCode { get; set; }
}

