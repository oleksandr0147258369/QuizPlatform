using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;

[Table("Questions")]
public class Question
{
    [Key] public int QuestionId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public int TestId { get; set; }
    [ForeignKey("TestId")] public virtual Test Test { get; set; }
}