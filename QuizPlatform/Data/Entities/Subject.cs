using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;

[Table("Subjects")]
public class Subject
{
    [Key] public int SubjectId { get; set; }
    [Required, MaxLength(100)] public string Name { get; set; }
}