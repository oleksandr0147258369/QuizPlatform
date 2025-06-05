using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("TestSessions")]
public class TestSession
{
    [Key] public int SessionId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int TestId { get; set; }
    [Required] public DateTime StartedAt { get; set; }
    [Required] public DateTime FinishedAt { get; set; }
    [ForeignKey("TestId")] public virtual Test Test { get; set; }
    [ForeignKey("UserId")] public virtual User User { get; set; }
}