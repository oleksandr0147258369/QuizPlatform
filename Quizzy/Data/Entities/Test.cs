using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quizzy.Data.Entities.Identity;

namespace Quizzy.Data.Entities;
[Table("Tests")]
public class Test
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)] public int TestId { get; set; }
    [Required] public string Name { get; set; }
    public string? Description { get; set; }
    [Required] public int CreatedById { get; set; }
    [Required] public DateTime CreatedUtc { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int GradeId { get; set; }
    [Required] public bool IsPrivate { get; set; }
    [Required] public bool IsCopyable { get; set; }
    [Required] public int QuestionsQuantity { get; set; }
    
    [ForeignKey( "CreatedById")] public virtual UserEntity CreatedBy { get; set; }
    [ForeignKey("SubjectId")] public virtual Subject Subject { get; set; }
    [ForeignKey("GradeId")] public virtual Grade Grade { get; set; }
    
    public virtual List<Question> Questions { get; set; }
    public bool IsPublished { get; set; }
}