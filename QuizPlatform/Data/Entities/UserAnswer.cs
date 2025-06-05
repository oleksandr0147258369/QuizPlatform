using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("UserAnswers")]
public class UserAnswer
{
    [Key] public int UserAnswerId { get; set; }
    [Required] public int SessionId { get; set; }
    [Required] public int QuestionId { get; set; }
    [Required] public int AnswerId { get; set; }
    [ForeignKey("SessionId")] public virtual TestSession Session { get; set; }
    [ForeignKey("QuestionId")] public virtual Question Question { get; set; }
    [ForeignKey("AnswerId")] public virtual Answer Answer { get; set; }
}