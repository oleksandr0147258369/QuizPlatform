using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace Quizzy.Data.Entities;

[Table("Questions")]
public class Question
{
    [Key] public int QuestionId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public bool HasMultipleCorrect { get; set; }
    [Required] public int TestId { get; set; }
    [Required] public int Points { get; set; }

    [ForeignKey("TestId")] public virtual Test Test { get; set; } = null!;
    
    public virtual List<Answer> Answers { get; set; }
}