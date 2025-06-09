using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Data.Entities;
using Quizzy.net.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data;

namespace Quizzy.net.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private static ConcurrentDictionary<long, User> Registration = new ConcurrentDictionary<long, User>();
    private static ConcurrentDictionary<long, string> VerCode = new ConcurrentDictionary<long, string>();
    private static long UniqueId = -9223372036854775808 + 1;
    private static ApplicationDbContext _db;
    
    public HomeController(ILogger<HomeController> logger, ApplicationDbContext dbContext)
    {
        _logger = logger;
        _db = dbContext;
    }

    public IActionResult Index()
    {
        return View("Home");
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }
    
    public IActionResult SignUp1()
    {
        
        return View();
    }
    
    public IActionResult SignUp2(long regId)
    {
        return View(new VerificationViewModel
        {
            RegId = regId,
            Error = null,
            Email = String.Empty
        });
    }

    public IActionResult SignUp3(long regId, string email)
    {
        return View(new VerificationViewModel
        {
            RegId = regId,
            Error = null,
            Email = email
        });
    }
    
    public IActionResult SignUp4(long regId)
    {
        return View(new VerificationViewModel
        {
            RegId = regId,
            Error = null,
            Email = String.Empty
        });
    }
    
    public IActionResult SignIn()
    {
        return View(new VerificationViewModel());
    }

    
    
    
    
    
    
    
    
    
    
    
    
    [HttpPost]
    public IActionResult SubmitSignUp1(string Name, string LastName, string MiddleName)
    {
        UniqueId++;


        foreach (var reg in Registration)
        {
            Console.WriteLine($"{reg.Value} - {reg.Key}");
        }
        
        Console.WriteLine($"{Registration.Count} users registered");
        
        string n1 = Name?.Trim() ?? "";
        string n2 = LastName?.Trim() ?? "";
        string n3 = "";

        if (!string.IsNullOrEmpty(MiddleName))
        {
            n3 = MiddleName.Trim();
        }

        if (n1.Count(c => c == ' ') >= 1 || n2.Count(c => c == ' ') >= 1 || n3.Count(c => c == ' ') >= 1)
        {
            ViewBag.Error = "Only one word is allowed.";
            return View("SignUp1");
        }

        Registration.TryAdd(UniqueId, new User());

        Registration[UniqueId].FirstName = Name.Trim();
        Registration[UniqueId].LastName = LastName.Trim();
        if (!string.IsNullOrEmpty(MiddleName))
        {
            Registration[UniqueId].MiddleName = MiddleName.Trim();
        }
        return RedirectToAction("SignUp2", new { regId = UniqueId });

    }
    
    
    
    
    [HttpPost]
    public async Task<IActionResult> SubmitSignUp2(string Email, long registrationId)
    {
        if (await _db.Users.AnyAsync(u => u.Email == Email))
        {
            return View("SignUp2", new VerificationViewModel
            {
                RegId = registrationId,
                Error = "User with that email already exists!",
                Email = String.Empty
            });
        }

        Registration[registrationId].Email = Email;
        string code = GenerateRandomCode(8);
        string name = Registration[registrationId].FirstName + " " + Registration[registrationId].LastName;
        SendEmail(Email, code, name);
        VerCode[registrationId] = code;
        
        return RedirectToAction("SignUp3", new { regId = registrationId, email = Email });
    }


    [HttpPost]
    public IActionResult SubmitSignUp3(long regId, string verificationCode)
    {
        Console.WriteLine($"Verification code: {verificationCode}");
        Console.WriteLine($"Registration Id: {regId}");

        if (!VerCode.ContainsKey(regId) || VerCode[regId].ToLower() != verificationCode.ToLower())
        {
            return View("SignUp3", new VerificationViewModel
            {
                RegId = regId,
                Email = Registration[regId].Email,
                Error = "Invalid code"
            });
        }

        return RedirectToAction("SignUp4", new {regId = regId});
    }
    
    
    [HttpPost]
    public async Task<IActionResult> SubmitSignUp4(long regId, bool IsTeacher, string Password)
    {
        try
        {
            Registration[regId].Password = PasswordHelper.HashPassword(Password);
            Registration[regId].Role = IsTeacher ? "Teacher" : "Student";
            Registration[regId].CreatedUtc = DateTime.UtcNow;
            Registration[regId].HasPhoto = false;
            Registration[regId].PhotoPath = "";

            await _db.Users.AddAsync(Registration[regId]);
            await _db.SaveChangesAsync();
        }
        catch (Exception e)
        {
            return View("SignUp4", new VerificationViewModel
            {
                RegId = regId,
                Email = String.Empty,
                Error = "Error occured"
            });
        }
        
        
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> SubmitSignIn(string Email, string Password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == Email);
        if (user != null && PasswordHelper.VerifyPassword(user.Password, Password))
        {
            return RedirectToAction("Index");
        }
        else
        {
            return View("SignIn", new VerificationViewModel
            {
                RegId = 0,
                Email = String.Empty,
                Error = "Incorrect Password or Email"
            });
        }
    }
    
    
    








   static void SendEmail(string email, string code, string name)
   {
       var fromAddress = new MailAddress("quizzytests@gmail.com", "Quizzy");
       var toAddress = new MailAddress(email, "Dear " + name);
       const string fromPassword = "eplc imyv edac hxzn"; 
       const string subject = "Verification code";
       string body = $"Hello dear {name}, here is your code - {code}, copy it and paste it on our website";
   
       var smtp = new SmtpClient
       {
           Host = "smtp.gmail.com",
           Port = 587, 
           EnableSsl = true,
           DeliveryMethod = SmtpDeliveryMethod.Network,
           UseDefaultCredentials = false,
           Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
       };
   
       using var message = new MailMessage(fromAddress, toAddress)
       {
           Subject = subject,
           Body = body
       };
   
       try
       {
           smtp.Send(message);
           Console.WriteLine("Email sent successfully.");
       }
       catch (Exception ex)
       {
           Console.WriteLine("Error: " + ex.Message);
       }
   }


   static string GenerateRandomCode(int length)
   {
       const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
       var random = new Random();
       var result = new char[length];

       for (int i = 0; i < length; i++)
       {
           result[i] = chars[random.Next(chars.Length)];
       }

       return new string(result);
   }

   [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

public class VerificationViewModel
{
    public long RegId { get; set; }
    public string? Error { get; set; }
    public string Email { get; set; }
}

public static class PasswordHelper
{
    private static readonly PasswordHasher<object> hasher = new PasswordHasher<object>();

    public static string HashPassword(string password)
    {
        return hasher.HashPassword(null, password);
    }

    public static bool VerifyPassword(string hashedPassword, string providedPassword)
    {
        var result = hasher.VerifyHashedPassword(null, hashedPassword, providedPassword);
        return result == PasswordVerificationResult.Success;
    }
}
