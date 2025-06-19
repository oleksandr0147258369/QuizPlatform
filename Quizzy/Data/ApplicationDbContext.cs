using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Quizzy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data.Entities.Identity;

namespace Quizzy.Data;

public class ApplicationDbContext : IdentityDbContext<UserEntity, RoleEntity, int>
{
    public DbSet<Answer> Answers { get; set; }
    public DbSet<Grade> Grades { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Result> Results { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<Subject> Subjects { get; set; }
    public DbSet<Test> Tests { get; set; }
    public DbSet<TestHomework> TestHomeworks { get; set; }
    public DbSet<TestSession> TestSessions { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Subject>().HasData(
            new Subject { SubjectId = 1, Name = "Mathematics" },
            new Subject { SubjectId = 2, Name = "History" },
            new Subject { SubjectId = 3, Name = "Physics" },
            new Subject { SubjectId = 4, Name = "Ukrainian language" },
            new Subject { SubjectId = 5, Name = "Ukrainian literature" },
            new Subject { SubjectId = 6, Name = "Geography" },
            new Subject { SubjectId = 7, Name = "Chemistry" }
        );

        modelBuilder.Entity<Grade>().HasData(
            new Grade { GradeId = 1, Number = 5 },
            new Grade { GradeId = 2, Number = 6 },
            new Grade { GradeId = 3, Number = 7 },
            new Grade { GradeId = 4, Number = 8 },
            new Grade { GradeId = 5, Number = 9 },
            new Grade { GradeId = 6, Number = 10 },
            new Grade { GradeId = 7, Number = 11 }
        );

        var createdDate = new DateTime(2024, 6, 13, 0, 0, 0, DateTimeKind.Utc);
        createdDate = DateTime.UtcNow;
        

        // identity 
        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        modelBuilder.Entity<UserRoleEntity>()
            .HasOne(ur => ur.Role)
            .WithMany(r => r.UserRoles)
            .HasForeignKey(ur => ur.RoleId);
    }
}