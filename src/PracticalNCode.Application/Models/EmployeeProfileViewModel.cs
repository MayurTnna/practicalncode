using System.ComponentModel.DataAnnotations;

namespace PracticalNCode.Application.Models;

public sealed class EmployeeProfileViewModel
{
    [Display(Name = "Employee Database ID")]
    public int EmployeeId { get; set; }

    [Display(Name = "Employee ID")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Display(Name = "Employee Name")]
    public string FullName { get; set; } = string.Empty;

    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Mobile Number")]
    public string MobileNumber { get; set; } = string.Empty;

    [Display(Name = "Department")]
    public string Department { get; set; } = string.Empty;

    [Display(Name = "Designation")]
    public string Designation { get; set; } = string.Empty;

    [Display(Name = "Organization")]
    public string OrganizationName { get; set; } = string.Empty;

    [Display(Name = "Created At")]
    public DateTime CreatedAt { get; set; }

    [Display(Name = "Last Updated")]
    public DateTime? UpdatedAt { get; set; }
}

