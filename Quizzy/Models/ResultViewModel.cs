namespace Quizzy.Models;

public class ResultViewModel
{
    public int Points { get; set; }
    public TimeSpan TimeSpent { get; set; }
    public TimeSpan TimePerQuestion { get; set; }
    public DateTime StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public string UserName { get; set; }
    public int MaxPoints {get; set; }
}