using Quizzy.Data.Entities;

namespace Quizzy.Models.Tests;

public class RunTestViewModel
{
    public List<Question> Questions { get; set; }
    
    public string Code { get; set; }


    public bool IsSuccessful { get; set; }


    public string Error { get; set; }


    public int SessionId { get; set; }


    public bool FullView { get; set; }
    
}