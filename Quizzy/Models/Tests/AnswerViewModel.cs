using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models.Tests;

public class AnswerViewModel
{
    public string? Text { get; set; }
    [Required]
    public bool IsCorrect { get; set; }
}