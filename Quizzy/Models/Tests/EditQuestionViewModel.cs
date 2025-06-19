using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models.Tests;

public class EditQuestionViewModel
{
    public int TestId { get; set; }
    public int QuestionId { get; set; }
    [Required(ErrorMessage = "Question Text is required")]
    public string Text { get; set; }
    public bool HasMultipleCorrect { get; set; }
    [Range(1, 100)]
    public int Points { get; set; }

    [MinLength(2, ErrorMessage = "At least two answers are required.")]
    public List<AnswerViewModel> Answers { get; set; } = new();
}