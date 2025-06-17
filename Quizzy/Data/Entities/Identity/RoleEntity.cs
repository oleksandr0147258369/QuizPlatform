using Microsoft.AspNetCore.Identity;

namespace Quizzy.Data.Entities.Identity;

public class RoleEntity : IdentityRole<int>
{
    public ICollection<UserRoleEntity> UserRoles { get; set; }
}