using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models;

public class RegisterViewModel
{
    [Display(Name = "First Name")]
    [Required(ErrorMessage = "First Name is required")]
    public string FirstName { get; set; }
    
    [Display(Name = "Middle Name")]
    public string? MiddleName { get; set; }
    
    [Display(Name = "Last Name")]
    [Required(ErrorMessage = "Last Name is required")]
    public string LastName { get; set; }
    
    [Display(Name = "Email Address")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    [DataType(DataType.EmailAddress)]
    [Required(ErrorMessage = "Email Address is required")]
    public string Email { get; set; }
    
    [Display(Name = "Verification Code")]
    [Required(ErrorMessage = "Verification Code is required")]
    public string UsersVerificationCode { get; set; }
    [Display(Name = "Password")]
    [DataType(DataType.Password)]
    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; }
    
    public bool IsTeacher { get; set; }
    
}