using Quizzy.Data.Entities;

namespace Quizzy.Models.Tests;

public class RunHomeworkTestViewModel
{
    public List<Question> Questions { get; set; }
    public TimeSpan? TimeToComplete { get; set; }
    public DateTime StatedAt { get; set; }
    public string Username { get; set; }
    public string HomeworkCode { get; set; }
    
    public string Code { get; set; }
    public bool IsSuccessful { get; set; }
    public string Error { get; set; }
    public int SessionId { get; set; }
    public bool FullView { get; set; }
}