using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models.Tests;

public class CreateViewModel
{
    [Required(ErrorMessage = "Test must have a name")]
    [Display(Name = "Name")]
    public string Name { get; set; }
    
    [Display(Name = "Subject")]
    public string Subject { get; set; }
    
    [Display(Name = "Grade")]
    public string Grade { get; set; }
}