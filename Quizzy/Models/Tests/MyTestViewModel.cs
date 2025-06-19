using Quizzy.Data.Entities.Identity;

namespace Quizzy.Models.Tests;

public class MyTestViewModel
{
    public int TestId { get; set; }
    public string Name { get; set; }
    public DateTime CreatedUtc { get; set; }
    public UserEntity CreatedBy { get; set; }
    public int QuestionsCount { get; set; }
}