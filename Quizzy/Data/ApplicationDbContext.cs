using Quizzy.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Quizzy.Data;

public class ApplicationDbContext : DbContext
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
    public DbSet<User> Users { get; set; }
    public DbSet<UserAnswer> UserAnswers { get; set; }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options){}
}