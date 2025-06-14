using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data;
using Quizzy.Data.Entities.Identity;
using Quizzy.Interfaces;
using Quizzy.Services;

namespace Quizzy;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        
        builder.Services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        builder.Services.AddIdentity<UserEntity, RoleEntity>(options =>
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
        app.UseRouting();
        app.MapStaticAssets();
        

        app.UseAuthorization();
        
        app.MapControllerRoute(
            
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
}