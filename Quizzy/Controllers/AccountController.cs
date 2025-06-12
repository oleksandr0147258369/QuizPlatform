using System.Collections.Concurrent;
using System.Diagnostics;
using System.Net;
using System.Net.Mail;
using System.Xml;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Quizzy.Data.Entities.Identity;
using Quizzy.Models;

namespace Quizzy.Controllers;

public class AccountController(UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    IMapper mapper) : Controller
{
    
    public IActionResult Preferences(int id)
    {
        return View();
    }

    // public IActionResult SavePreferences(UserViewModel model)
    // {
    //     var user = _db.Users.Find(model.Id);
    //     user.FirstName = model.FirstName;
    //     user.MiddleName = model.MiddleName;
    //     user.LastName = model.LastName;
    //     var school = _db.Schools.Find(model.SchoolName);
    //     if (school != null)
    //     {
    //         user.School = school;
    //         user.SchoolId = school.SchoolId;
    //     }
    //     
    //     user.Email = model.Email;
    //     user.PhoneNumber = model.PhoneNumber;
    //     user.About = model.About;
    //     
    //     _db.Users.Update(user);
    //     return Redirect("/Account/Preferences");
    // }


    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult SignUp1()
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp1(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        return View("SignUp2", model);
    }

    [HttpPost]
    public IActionResult SignUp2(RegisterViewModel model)
    {
        return View();
    }

    [HttpPost]
    public IActionResult SignUp4(long regId)
    {
        return View(new VerificationViewModel
        {
            RegId = regId,
            Error = null,
            Email = String.Empty
        });
    }
    [HttpGet]
    public IActionResult SignIn()
    {
        return View(new VerificationViewModel());
    }
    [HttpPost]
    
    [HttpPost]
    
    
    
    
    
    
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
