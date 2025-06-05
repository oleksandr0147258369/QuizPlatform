using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("Results")]
public class Result
{
    [Key] public int ResultId { get; set; }
    [Required] public int UserId { get; set; }
    [Required] public int TestId { get; set; }
    [Required] public int Score { get; set; }
    [ForeignKey("UserId")] public virtual User User { get; set; }
    [ForeignKey("TestId")] public virtual Test Test { get; set; }
}