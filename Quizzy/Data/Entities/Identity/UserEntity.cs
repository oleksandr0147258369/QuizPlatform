using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;

namespace Quizzy.Data.Entities.Identity;
[Table("Users")]
public class UserEntity : IdentityUser<int>
{
    [Required] public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    [Required] public string LastName { get; set; }
    public string? About { get; set; }
    public int? SchoolId { get; set; }
    [Required] public DateTime CreatedUtc { get; set; }
    [Required] public string Image { get; set; }
    
    [ForeignKey("SchoolId")] public virtual School? School { get; set; }
    
    public virtual List<Test> Tests { get; set; } = new();
    public virtual List<TestHomework> TestHomeworks { get; set; } = new();
    public virtual List<TestSession> TestSessions { get; set; } = new();
    
    public ICollection<UserRoleEntity>? UserRoles { get; set; } = new List<UserRoleEntity>();
}