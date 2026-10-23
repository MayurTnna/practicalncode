using System.ComponentModel.DataAnnotations;

namespace PracticalNCode.Application.Models;

public sealed class CreateEmployeeViewModel
{
    public int OrganizationId { get; set; }

    [Required(ErrorMessage = "Employee ID is required.")]
    [StringLength(50, ErrorMessage = "Employee ID cannot exceed 50 characters.")]
    [Display(Name = "Employee ID")]
    public string EmployeeCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "Employee Name is required.")]
    [StringLength(150, ErrorMessage = "Employee Name cannot exceed 150 characters.")]
    [Display(Name = "Employee Name")]
    public string FullName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email Address is required.")]
    [EmailAddress(ErrorMessage = "Invalid Email Address format.")]
    [StringLength(150, ErrorMessage = "Email Address cannot exceed 150 characters.")]
    [Display(Name = "Email Address")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Mobile Number is required.")]
    [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Please enter a valid 10-digit Indian mobile number.")]
    [Display(Name = "Mobile Number")]
    public string MobileNumber { get; set; } = string.Empty;

    [Required(ErrorMessage = "Department is required.")]
    [StringLength(100, ErrorMessage = "Department cannot exceed 100 characters.")]
    [Display(Name = "Department")]
    public string Department { get; set; } = string.Empty;

    [Required(ErrorMessage = "Designation is required.")]
    [StringLength(100, ErrorMessage = "Designation cannot exceed 100 characters.")]
    [Display(Name = "Designation")]
    public string Designation { get; set; } = string.Empty;
}

