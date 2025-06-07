using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("Schools")]
public class School
{
    [Key] public int SchoolId { get; set; }
    [Required] public string Name { get; set; }
    
    public virtual List<User> Users { get; set; }
}