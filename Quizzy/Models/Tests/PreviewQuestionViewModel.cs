namespace Quizzy.Models.Tests;

public class PreviewQuestionViewModel
{
    public string Text { get; set; }
    public bool HasMultipleCorrect { get; set; }
    public int Points { get; set; }
    public List<AnswerViewModel> Answers { get; set; }
}