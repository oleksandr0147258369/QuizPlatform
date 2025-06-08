using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace Quizzy.Data.Entities;

[Table("Questions")]
public class Question
{
    [Key] public int QuestionId { get; set; }
    [Required] public bool HasText { get; set; }
    public string? Text { get; set; }
    [Required] public bool HasMultipleCorrect { get; set; }
    [Required] public int TestId { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int GradeId { get; set; }
    [Required] public bool HasPhoto  { get; set; }
    public string? PhotoPath { get; set; }
    [Required] public int Points { get; set; }
    [Required] public int Order { get; set; }

    [ForeignKey("TestId")] public virtual Test Test { get; set; } = null!;
    [ForeignKey("SubjectId")] public virtual Subject Subject { get; set; } = null!;
    [ForeignKey("GradeId")] public virtual Grade Grade { get; set; } = null!;
    
    public virtual List<Answer> Answers { get; set; }
}