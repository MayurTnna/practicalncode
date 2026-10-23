using System.ComponentModel.DataAnnotations;

namespace PracticalNCode.Application.Models;

public sealed class CreateOrganizationViewModel
{
    [Required(ErrorMessage = "Organization Name is required.")]
    [StringLength(150, MinimumLength = 2, ErrorMessage = "Organization Name must be between 2 and 150 characters.")]
    [Display(Name = "Organization Name")]
    public string OrganizationName { get; set; } = string.Empty;

    [EmailAddress(ErrorMessage = "Invalid Contact Email format.")]
    [StringLength(150, ErrorMessage = "Email cannot exceed 150 characters.")]
    [Display(Name = "Contact Email")]
    public string? ContactEmail { get; set; }

    [RegularExpression(
      @"^[6-9]\d{9}$",
      ErrorMessage = "Please enter a valid 10-digit Indian mobile number.")]
    [Display(Name = "Contact Phone")]
    public string? ContactPhone { get; set; }

    [Required(ErrorMessage = "Login Username is required.")]
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

