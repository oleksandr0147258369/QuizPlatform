using Microsoft.EntityFrameworkCore;
using Quizzy.Data;

namespace Quizzy;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddControllersWithViews();
        builder.Services.AddRazorPages();
        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
        
        var app = builder.Build();
        var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseStaticFiles();

        app.UseAuthorization();

        app.UseStaticFiles();
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        
        
    }
}