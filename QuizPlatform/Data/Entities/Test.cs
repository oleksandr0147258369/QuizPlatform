using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("Tests")]
public class Test
{
    [Key] public int TestId { get; set; }
    [Required, MaxLength(255)] public string Title { get; set; }
    public string Description { get; set; }
    [Required] public int CategoryId { get; set; }
    [Required] public int SubjectId { get; set; }
    [Required] public int CreatedById { get; set; }
    [Required] public DateTime CreatedAt { get; set; }
    [ForeignKey("CategoryId")] public virtual Category Category { get; set; }
    [ForeignKey("SubjectId")] public virtual Subject Subject { get; set; }
    [ForeignKey("CreatedById")] public virtual User CreatedBy { get; set; }
}