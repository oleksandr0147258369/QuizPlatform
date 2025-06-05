using System.Text;
using QuizPlatform.Data;
using QuizPlatform.Data.Entities;

namespace QuizPlatform;

public class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello World!");
        var context = new ApplicationDbContext();
        context.Users.Add(new User {Username = "test", Email = "test@test.com", Role = "test", PasswordHash = "test"});
        context.SaveChanges();
    }
}