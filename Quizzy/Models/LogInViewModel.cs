using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models;

public class LogInViewModel
{
    [Required (ErrorMessage = "Email is required")]
    [EmailAddress (ErrorMessage = "Invalid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; }
    
    [Required (ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }
}