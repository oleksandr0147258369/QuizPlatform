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
        Console.WriteLine("Connection string: " + context.Database.GetDbConnection().ConnectionString);


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
        
        app.MapControllerRoute(
            name: "signup",
            pattern: "Account/signup1",
            defaults: new { controller = "Account", action = "SignUp1" });
        
        app.MapControllerRoute(
            name: "signup",
            pattern: "Account/signup2",
            defaults: new { controller = "Account", action = "SignUp2" });
        
        app.MapControllerRoute(
            name: "signup",
            pattern: "Account/signup3",
            defaults: new { controller = "Account", action = "SignUp3" });
        app.MapControllerRoute(
            name: "signup",
            pattern: "Account/signup4",
            defaults: new { controller = "Account", action = "SignUp4" });
        
        app.MapControllerRoute(
            name: "signin",
            pattern: "Account/signin",
            defaults: new { controller = "Account", action = "SignIn" });
        
        
        app.MapControllerRoute(
            name: "SearchTests",
            pattern: "Tests/Search_Tests",
            defaults: new { controller = "Home", action = "Search_Test" });
        
        


        app.MapControllerRoute(
            
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}");
        app.Run();
    }
}