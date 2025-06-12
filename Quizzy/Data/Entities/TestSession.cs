using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Data.Entities;
[Table("TestSessions")]
public class TestSession
{
    [Key] public int TestSessionId { get; set; }
    [Required] public int UserId { get; set; }
    
    [Required] public bool IsTestHomework { get; set; }
    public int? TestId { get; set; }
    public int? TestHomeworkId { get; set; }
    
    [Required] public DateTime StartedAt { get; set; }
    [Required] public bool IsFinished { get; set; }
    public DateTime? FinishedAt { get; set; }
    
    [ForeignKey("TestId")] public virtual Test? Test { get; set; }
    [ForeignKey("UserId")] public virtual UserEntity UserEntity { get; set; }
    [ForeignKey("TestHomeworkId")] public virtual TestHomework TestHomework { get; set; }
    
    public virtual List<UserAnswer> UserAnswers { get; set; }
}