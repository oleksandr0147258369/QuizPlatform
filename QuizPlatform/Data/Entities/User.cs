using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace QuizPlatform.Data.Entities;
[Table("Users")]
public class User
{
    [Key] public int UserId { get; set; }
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    [Required] public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? About { get; set; }
    [Required] public string Role { get; set; }
    public int? SchoolId { get; set; }
    [Required] public DateTime CreatedUtc { get; set; }
    [Required] public bool HasPhoto { get; set; }
    [Required] public string PhotoPath { get; set; }
    [Required] public string Password { get; set; }
    
    [ForeignKey("SchoolId")] public virtual School? School { get; set; }
    
    public virtual List<Test> Tests { get; set; }
    public virtual List<TestHomework> TestHomeworks { get; set; }
    public virtual List<TestSession> TestSessions { get; set; }
}