using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Data.Entities;

[Table("Subjects")]
public class Subject
{
    [Key] public int SubjectId { get; set; }
    [Required] public string Name { get; set; }
    
    public virtual List<Question> Questions { get; set; }
    public virtual List<Test> Tests { get; set; }
}