using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("Answers")]
public class Answer
{
    [Key] public int AnswerId { get; set; }
    [Required] public int QuestionId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public bool IsCorrect { get; set; }
    [ForeignKey("QuestionId")] public virtual Question Question { get; set; }
}