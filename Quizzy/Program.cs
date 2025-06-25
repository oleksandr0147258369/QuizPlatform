using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Quizzy.Data.Entities.Identity;
using Quizzy.Interfaces;
using Quizzy.Services;

namespace Quizzy;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();

        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services
            .AddIdentity<UserEntity, RoleEntity>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 6;
                options.Password.RequiredUniqueChars = 1;
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        builder.Services.AddScoped<ISMTPService, SMTPService>();
        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

        var app = builder.Build();

        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        
        // using (var scope = app.Services.CreateScope())
        // {
        //     var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //     await SeedSampleTest(dbContext); 
        // }
        app.Run();
    }

    
   public static async Task SeedSampleTest(ApplicationDbContext _db)
{
    if (_db.Tests.Any(t => t.Name == "Sample Test"))
    {
        _db.Tests.RemoveRange(_db.Tests);
    }

    var test = new Test
    {
        Name = "Sample Test",
        Description = "A small test for demo purposes",
        CreatedById = 5,  // ❗ Упевнись, що User з ID 5 існує
        CreatedUtc = DateTime.UtcNow,
        SubjectId = 1,    // ❗ Перевір, що Subject з ID 1 існує
        GradeId = 1,      // ❗ Перевір, що Grade з ID 1 існує
        IsPrivate = false,
        IsCopyable = false,
        IsPublished = true,
        QuestionsQuantity = 3,
        Questions = new List<Question>() // 🟢 ВАЖЛИВО!
    };

    var questions = new List<Question>
    {
        new Question
        {
            Text = "Якого кольору небо?",
            HasMultipleCorrect = false,
            Points = 1,
            Answers = new List<Answer>
            {
                new Answer { Text = "Синій", IsCorrect = true },
                new Answer { Text = "Червоний", IsCorrect = false },
                new Answer { Text = "Жовтий", IsCorrect = false }
            }
        },
        new Question
        {
            Text = "Вибери всі парні числа:",
            HasMultipleCorrect = true,
            Points = 2,
            Answers = new List<Answer>
            {
                new Answer { Text = "1", IsCorrect = false },
                new Answer { Text = "2", IsCorrect = true },
                new Answer { Text = "3", IsCorrect = false },
                new Answer { Text = "4", IsCorrect = true }
            }
        },
        new Question
        {
            Text = "Столиця Франції?",
            HasMultipleCorrect = false,
            Points = 1,
            Answers = new List<Answer>
            {
                new Answer { Text = "Лондон", IsCorrect = false },
                new Answer { Text = "Париж", IsCorrect = true },
                new Answer { Text = "Берлін", IsCorrect = false }
            }
        }
    };

    // Додаємо питання до тесту
    foreach (var question in questions)
    {
        test.Questions.Add(question);
    }

    _db.Tests.Add(test);
    await _db.SaveChangesAsync();
}


}