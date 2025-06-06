using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace QuizPlatform.Data.Entities;

[Table("Questions")]
public class Question
{
    [Key] public int QuestionId { get; set; }
    [Required] public string Text { get; set; }
    [Required] public int TestId { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int CategoryId { get; set; }
    [ForeignKey("TestId")] public virtual Test Test { get; set; }
    [ForeignKey("SubjectId")] public virtual Subject Subject { get; set; }
    [ForeignKey("CategoryId")] public virtual Category Category { get; set; }

    public virtual List<Answer> Answers { get; set; } = new();
}