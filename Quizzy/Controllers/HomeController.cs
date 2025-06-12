using System.Collections.Concurrent;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Data.Entities;
using Quizzy.Models;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Identity;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Quizzy.Controllers;

public class HomeController : Controller
{
    private static ApplicationDbContext _db;

    public HomeController(ApplicationDbContext dbContext)
    {
        
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
        return View(new VerificationContactModel
        {
            IsSuccess = false,
            Error = null
        });
    }

    public IActionResult Search_Test()
    {
        return View();
    }


    public IActionResult Search(string query, string subject, int? classNumber)
    {
        var results = _db.Tests.AsQueryable();

        if (!string.IsNullOrEmpty(query))
        {
            results = results.Where(t => t.Name.Contains(query) || t.Description.Contains(query));
        }
        if (!string.IsNullOrEmpty(subject))
        {
            results = results.Where(t => t.Subject.Name == subject);
        }
        if (classNumber.HasValue)
        {
            results = results.Where(t => t.GradeId == classNumber.Value);
        }

        return View(results.ToList());
    }



    public IActionResult Contact_Submit(string Email, string Message, string Name)
    {
        try
        {
            SendEmail(Email, Name, Message);
            return View("Contact", new VerificationContactModel
            {
                IsSuccess = true
            });
        }
        catch (Exception ex)
        {
            return View("Contact", new VerificationContactModel
            {
                Error = "Something went wrong",
                IsSuccess = false
            });
        }
    }
    
    static void SendEmail(string email, string name, string message1)
    {
        var fromAddress = new MailAddress("quizzytests@gmail.com", "Quizzy");
        var toAddress = new MailAddress("quizzytests@gmail.com");
        const string fromPassword = "eplc imyv edac hxzn"; 
        const string subject = "UserEntity Message";
        string body = $"Message from {name} - {email}:\n{message1}";
   
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

}









    


    
    
    
    








   
