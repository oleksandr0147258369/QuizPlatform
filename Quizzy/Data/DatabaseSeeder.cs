using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizzy.Constants;
using Quizzy.Data.Entities;
using Quizzy.Data.Entities.Identity;
using Quizzy.Interfaces;

namespace Quizzy.Data;

public static class DatabaseSeeder
{
    public static async Task SeedData(this WebApplication webApplication)
    {
        using var scope = webApplication.Services.CreateScope();
        //Цей об'єкт буде верта посилання на конткетс, який зараєстрвоано в Progran.cs
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<UserEntity>>();

        var mapper = scope.ServiceProvider.GetRequiredService<IMapper>();
        var smtpService = scope.ServiceProvider.GetRequiredService<ISMTPService>();

        context.Database.Migrate();
        if (!context.Roles.Any())
        {
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<RoleEntity>>();
            var admin = new RoleEntity { Name = Roles.Admin };
            var result = await roleManager.CreateAsync(admin);
            if (result.Succeeded)
            {
                Console.WriteLine($"Роль {Roles.Admin} створено успішно");
            }
            else
            {
                Console.WriteLine($"Помилка створення ролі:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
            }

            var student = new RoleEntity { Name = Roles.Student };
            result = await roleManager.CreateAsync(student);
            if (result.Succeeded)
            {
                Console.WriteLine($"Роль {Roles.Student} створено успішно");
            }
            else
            {
                Console.WriteLine($"Помилка створення ролі:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
            }

            var teacher = new RoleEntity { Name = Roles.Teacher };
            result = await roleManager.CreateAsync(teacher);
            if (result.Succeeded)
            {
                Console.WriteLine($"Роль {Roles.Teacher} створено успішно");
            }
            else
            {
                Console.WriteLine($"Помилка створення ролі:");
                foreach (var error in result.Errors)
                {
                    Console.WriteLine($"- {error.Code}: {error.Description}");
                }
            }
        }

        if (!context.Tests.Any())
        {
            Console.WriteLine("dodaju");
            var createdDate = DateTime.UtcNow;
            var tests = new List<Test>()
            {
                new Test
                {
                    TestId = 1,
                    Name = "Basic Mathematics Quiz",
                    Description = "A simple math quiz for beginners.",
                    CreatedById = 1,
                    CreatedUtc = createdDate,
                    SubjectId = 1,
                    GradeId = 5,
                    IsPrivate = false,
                    IsCopyable = true,
                    QuestionsQuantity = 10
                },
                new Test
                {
                    TestId = 2,
                    Name = "World History Challenge",
                    Description = "Test your knowledge of ancient civilizations.",
                    CreatedById = 1,
                    CreatedUtc = createdDate,
                    SubjectId = 2,
                    GradeId = 7,
                    IsPrivate = false,
                    IsCopyable = false,
                    QuestionsQuantity = 15
                },
                new Test
                {
                    TestId = 3,
                    Name = "Physics Fundamentals Test",
                    Description = "A physics quiz focusing on mechanics and motion.",
                    CreatedById = 1,
                    CreatedUtc = createdDate,
                    SubjectId = 3,
                    GradeId = 2,
                    IsPrivate = true,
                    IsCopyable = true,
                    QuestionsQuantity = 20
                }
            };
            context.Tests.AddRange(tests);
            await context.SaveChangesAsync();
        }
    }
}