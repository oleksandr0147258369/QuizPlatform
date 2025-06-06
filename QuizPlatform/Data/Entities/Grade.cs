using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;

[Table("Grades")]
public class Category
{
    [Key] public int GradeId { get; set; }
    [Required] public int SubjectId { get; set; }
    [ForeignKey("SubjectId")] public virtual Subject Subject { get; set; }
    
    public virtual List<Question> Questions { get; set; }
}