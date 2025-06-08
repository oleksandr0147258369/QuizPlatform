using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Data.Entities;
[Table("Answers")]
public class Answer
{
    [Key] public int AnswerId { get; set; }
    [Required] public int QuestionId { get; set; }
    public string? Text { get; set; }
    [Required] public bool IsCorrect { get; set; }
    [Required] public bool HasText { get; set; }
    [Required] public bool HasPhoto { get; set; }
    public string? PhotoPath { get; set; }
    [Required] public int Order { get; set; }
    
    [ForeignKey("QuestionId")] public virtual Question Question { get; set; }
}