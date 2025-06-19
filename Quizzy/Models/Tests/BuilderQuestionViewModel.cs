using System.ComponentModel.DataAnnotations;

namespace Quizzy.Models.Tests;

public class BuilderQuestionViewModel
{
    public int Id { get; set; }
    public string Text { get; set; }
    public bool HasMultipleCorrect { get; set; }
    public int Points { get; set; }
    public List<AnswerViewModel> Answers { get; set; }
}