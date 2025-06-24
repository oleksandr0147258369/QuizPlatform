using Quizzy.Data.Entities.Identity;

namespace Quizzy.Models.Tests;

public class PreviewViewModel
{
    public int TestId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedUtc { get; set; }
    public UserEntity CreatedBy { get; set; }
    public int QuestionsCount { get; set; }
    public List<PreviewQuestionViewModel> Questions { get; set; } = new ();
}