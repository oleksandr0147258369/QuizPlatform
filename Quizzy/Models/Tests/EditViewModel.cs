using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models.Tests;

public class EditViewModel
{
    public int TestId { get; set; }
    [Required(ErrorMessage = "You must enter a name")]
    [Display(Name = "Name")]
    public string Name { get; set; }
    [Display(Name = "Subject")]
    public string Subject { get; set; }
    [Display(Name = "Grade")]
    public string Grade { get; set; }
    [Display(Name = "Description")]
    public string? Description { get; set; }
    [Display(Name = "Privacy")]
    public bool IsPrivate { get; set; }
    [Display(Name = "Allow to copy test")]
    public bool IsCopyable { get; set; }
}