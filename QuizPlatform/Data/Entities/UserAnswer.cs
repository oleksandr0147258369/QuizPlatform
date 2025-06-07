using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("UserAnswers")]
public class UserAnswer
{
    [Key] public int UserAnswerId { get; set; }
    [Required] public int AnswerId { get; set; }
    [Required] public int TestSessionId { get; set; }
    
    [ForeignKey("AnswerId")] public virtual Answer Answer { get; set; }
    [ForeignKey("TestSessionId")] public virtual TestSession TestSession { get; set; }
}