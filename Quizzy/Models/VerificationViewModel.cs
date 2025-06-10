namespace Quizzy.Models;

public class VerificationViewModel
{
    public long RegId { get; set; }
    public string? Error { get; set; }
    public string Email { get; set; }
}