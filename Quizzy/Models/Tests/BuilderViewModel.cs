namespace Quizzy.Models.Tests;

public class BuilderViewModel
{
    public string Name { get; set; }
    public bool IsPrivate { get; set; }
    public int QuestionsCount { get; set; }
    public int MaxPoints { get; set; }
    public List<BuilderQuestionViewModel> Questions { get; set; }
}