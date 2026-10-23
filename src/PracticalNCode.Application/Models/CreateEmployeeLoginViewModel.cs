using System.ComponentModel.DataAnnotations;

namespace PracticalNCode.Application.Models;

public sealed class CreateEmployeeLoginViewModel
{
    [Required(ErrorMessage = "Please select an employee.")]
    [Range(1, int.MaxValue, ErrorMessage = "Please select a valid employee.")]
    [Display(Name = "Employee")]
    public int EmployeeId { get; set; }

    [Display(Name = "Employee Name")]
    public string? EmployeeName { get; set; }

    [Display(Name = "Employee Code / ID")]
    public string? EmployeeCode { get; set; }

    [Display(Name = "Organization")]
    public string? OrganizationName { get; set; }

    [Display(Name = "Employee Email")]
    public string? EmployeeEmail { get; set; }

    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 100 characters.")]
    [Display(Name = "Login Username")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters.")]
    [RegularExpression(@"^(?=.*\d)(?=.*[^a-zA-Z0-9]).+$", ErrorMessage = "Password must contain at least one digit and one special character.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Confirm Password is required.")]
    [DataType(DataType.Password)]
    [Compare(nameof(Password), ErrorMessage = "The password and confirmation password do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; } = string.Empty;
}

