using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("TestHomeworks")]
public class TestHomework
{
    [Key] public int TestHomeworkId { get; set; }
    [Required] public int CreatedById { get; set; }
    [Required] public int TestId { get; set; }
    [Required] public DateTime CreatedUtc { get; set; }
    [Required] public bool HasDeadline { get; set; }
    public DateTime? Deadline { get; set; }
    [Required] public bool HasTimeToComplete { get; set; }
    public TimeSpan? TimeToComplete { get; set; }
    
    [ForeignKey("CreatedById")] public User CreatedBy { get; set; }
    [ForeignKey("TestId")] public Test Test { get; set; }
}