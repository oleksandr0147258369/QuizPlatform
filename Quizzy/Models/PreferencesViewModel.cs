using System.ComponentModel.DataAnnotations;
using Quizzy.Data.Entities;

namespace Quizzy.Models;

public class PreferencesViewModel
{
    [Display(Name = "Email Address")]
    public string Email { get; set; }
    public string? PhotoName { get; set; }
    [Display(Name ="Upload Photo")]
    public IFormFile? Photo { get; set; }
    [Required(ErrorMessage = "You must have a First Name")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }
    [Display(Name = "Middle Name")]
    public string? MiddleName { get; set; }
    [Required(ErrorMessage = "You must have a Last Name")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }
    [Display(Name = "About Me")]
    public string? About { get; set; }
    
    [Display(Name = "School")]
    public string? School { get; set; }
    
    public List<string>? Schools { get; set; }
    
    [Display(Name = "Old Password")]
    public string? OldPassword { get; set; }
    [Display(Name = "New Password")]
    public string? NewPassword { get; set; }
    [Display(Name = "Confirm Password")]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
    public string? ConfirmPassword { get; set; }
    
    
    public bool second_error { get; set; }
}