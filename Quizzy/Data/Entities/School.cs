using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Quizzy.Data.Entities.Identity;

namespace Quizzy.Data.Entities;
[Table("Schools")]
public class School
{
    [Key] public int SchoolId { get; set; }
    [Required] public string Name { get; set; }
    
    public virtual List<UserEntity> Users { get; set; }
}