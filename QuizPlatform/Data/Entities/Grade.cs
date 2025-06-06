using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;

[Table("Grades")]
public class Grade
{
    [Key] public int GradeId { get; set; }
    [Required] public int Number { get; set; }
    
    public virtual List<Question> Questions { get; set; }
    public virtual List<Test> Tests { get; set; }
}