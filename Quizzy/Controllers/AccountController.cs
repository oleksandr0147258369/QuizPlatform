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
using Quizzy.Services;

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
        Console.WriteLine(model.FirstName + " " + model.LastName);
        if (string.IsNullOrEmpty(model.LastName))
        {
            ModelState.AddModelError("", "Last name is required.");
        }

        if (string.IsNullOrEmpty(model.FirstName))
        {
            ModelState.AddModelError("", "First name is required.");
        }
        if (string.IsNullOrEmpty(model.FirstName) || string.IsNullOrEmpty(model.LastName))
            return View(model);
        Console.WriteLine(model.FirstName + " " + model.LastName + "return");
        return View("SignUp2", model);
        // return RedirectToAction(nameof(SignUp2), "Account", model);
    }
    [HttpPost]
    public IActionResult SignUp2(RegisterViewModel model)
    {
        // Store user input from step 2
        TempData["FirstName"] = model.FirstName;
        TempData["LastName"] = model.LastName;
        TempData["MiddleName"] = model.MiddleName;
        TempData["Email"] = model.Email;

        // Generate and store code securely
        TempData["VerificationCode"] = GenerateRandomCode(8);

        return RedirectToAction("SignUp3Get");
    }

    [HttpGet]
    public IActionResult SignUp3Get()
    {
        var model = new RegisterViewModel
        {
            FirstName = TempData["FirstName"]?.ToString(),
            LastName = TempData["LastName"]?.ToString(),
            MiddleName = TempData["MiddleName"]?.ToString(),
            Email = TempData["Email"]?.ToString()
        };

        // Resave TempData for next request
        TempData.Keep();

        var code = TempData["VerificationCode"]?.ToString();
        TempData.Keep("VerificationCode");

        if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(code))
            return RedirectToAction("SignUp1"); // fallback

        new SMTPService().SendEmail(model.Email, code, model.FirstName);
        return View("SignUp3", model);
    }
    [HttpPost]
    public IActionResult SignUp3(RegisterViewModel model)
    {
        var userCode = model.UsersVerificationCode;
        Console.WriteLine(TempData.Peek("VerificationCode") + userCode);

        if (string.IsNullOrEmpty(userCode) || TempData.Peek("VerificationCode").ToString() != userCode)
        {
            ModelState.AddModelError("UsersVerificationCode", "Invalid verification code.");
            return View(model);
        }

        // Carry TempData forward for SignUp4
        TempData.Keep();

        return View("SignUp4", model);
    }
    [HttpPost]
    public  async Task<IActionResult> SignUp4(RegisterViewModel model)
    {
        var user = mapper.Map<UserEntity>(model);
        Console.WriteLine(user.UserName);
        Console.WriteLine(user.Email);
        Console.WriteLine(user.FirstName);
        Console.WriteLine(user.MiddleName);
        Console.WriteLine(user.LastName);
        Console.WriteLine(user.PasswordHash);
        Console.WriteLine(user.FirstName);
        var res = await userManager.CreateAsync(user, model.Password);
        if (res.Succeeded)
        {
            if (model.IsTeacher)
                await userManager.AddToRoleAsync(user, "Teacher");
            else await userManager.AddToRoleAsync(user, "Student");
            await signInManager.SignInAsync(user, isPersistent: false);
            return View("~/Views/Home/Home.cshtml");
        }

        foreach (var error in res.Errors)
        {
            ModelState.AddModelError("", error.Description);
        }
        return View(model);
    }
    [HttpGet]
    public IActionResult SignIn()
    {
        return View(new VerificationViewModel());
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