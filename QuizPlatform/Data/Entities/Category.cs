using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;

[Table("Categories")]
public class Category
{
    [Key] public int CategoryId { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; }
    [Required] public int SubjectId { get; set; }
    [ForeignKey("SubjectId")] public virtual Subject Subject { get; set; }
}