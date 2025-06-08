using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Quizzy.Data.Entities;
[Table("Results")]
public class Result
{
    [Key] public int ResultId { get; set; }
    [Required] public int TestSessionId { get; set; }
    [Required] public int Points { get; set; }
    [Required] public TimeSpan TimeSpent { get; set; }
    [Required] public TimeSpan TimePerQuestion { get; set; }
    
    [ForeignKey("TestSessionId")] public virtual TestSession TestSession { get; set; }
}