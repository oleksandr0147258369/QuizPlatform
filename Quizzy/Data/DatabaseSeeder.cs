using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizzy.Constants;
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
    }
}