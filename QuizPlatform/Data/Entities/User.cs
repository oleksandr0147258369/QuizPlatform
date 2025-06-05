using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizPlatform.Data.Entities;
[Table("Users")]
public class User
{
    [Key] public int UserId { get; set; }
    [Required, MaxLength(100)] public string Username { get; set; }
    [Required, MaxLength(255)] public string Email { get; set; }
    [Required, MaxLength(255)] public string PasswordHash { get; set; }
    [Required] public string Role { get; set; }
}