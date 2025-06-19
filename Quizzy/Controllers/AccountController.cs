using System.Diagnostics;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Quizzy.Data.Entities.Identity;
using Quizzy.Models;
using Quizzy.Services;

namespace Quizzy.Controllers;

public class AccountController(UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    IMapper mapper, ApplicationDbContext db) : Controller
{
    
    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await signInManager.SignOutAsync();
        Console.WriteLine("User logged out.");
        return RedirectToAction("Index", "Home");
    }
    [HttpGet]
    public async Task<IActionResult> Preferences()
    {
        var user = await userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account");
        var model = mapper.Map<PreferencesViewModel>(user);
        model.Schools = db.Schools.Select(s => s.Name).ToList();
        Console.WriteLine("model image: " + model.PhotoName);
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> Preferences(PreferencesViewModel model)
    {
        if (!ModelState.IsValid)
        {
            
            model.Schools = db.Schools.Select(s => s.Name).ToList();
            return View(model);
        }

        var user = await userManager.GetUserAsync(User);
        mapper.Map(model, user);

        if (!string.IsNullOrWhiteSpace(model.School))
        {
            var school = db.Schools.FirstOrDefault(s => s.Name == model.School);
            if (school == null)
            {
                school = new School { Name = model.School };
                db.Schools.Add(school);
                await db.SaveChangesAsync();
            }

            user.SchoolId = school.SchoolId;
        }

        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            var result = await userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    Console.WriteLine(error.Description);
                }

                Console.WriteLine("errorchik");
                model = mapper.Map(user, model);
                model.Schools = db.Schools.Select(s => s.Name).ToList();
                return View(model);
            }
        }

        if (model.Photo != null && model.Photo.Length > 0)
        {
            Console.WriteLine("got into photo saving block");
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/users");
            Directory.CreateDirectory(uploadsFolder);

            var fileExt = Path.GetExtension(model.Photo.FileName);
            var fileName = $"{Guid.NewGuid()}{fileExt}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await model.Photo.CopyToAsync(fileStream);
            }
            if (user.Image != "default.png")
                System.IO.File.Delete(Path.Combine(uploadsFolder, user.Image));
            user.Image = fileName;
            model.PhotoName = fileName;
        }

        await userManager.UpdateAsync(user);
        await db.SaveChangesAsync();
        ViewBag.Success = "Changes saved successfully!";
        model = mapper.Map<PreferencesViewModel>(user);
        model.Schools = db.Schools.Select(s => s.Name).ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult LogIn()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> LogIn(LogInViewModel model)
    {
        if(!ModelState.IsValid)
            return View(model);
        var user = await userManager.FindByEmailAsync(model.Email);
        if (user != null)
        {
            var res = await signInManager.PasswordSignInAsync(user, model.Password, false, false);
            if (res.Succeeded)
            {
                await signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/");
            }
        }
        ModelState.AddModelError("Password", "Invalid login attempt.");
        return View(model);
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
        // if (!ModelState.IsValid)
        // {
        //     foreach (var error in res.Errors)
        //     {
        //         if (error.Code.Contains("Password"))
        //             ModelState.AddModelError(nameof(model.Password), error.Description);
        //         else
        //             ModelState.AddModelError(string.Empty, error.Description);
        //     }
        //     return View(model);
        // }
        var user = mapper.Map<UserEntity>(model);
        user.Image = "default.png";
        user.CreatedUtc = DateTime.UtcNow;
        user.EmailConfirmed = true;
        var res = await userManager.CreateAsync(user, model.Password);
        if (res.Succeeded)
        {
            if (model.IsTeacher)
                await userManager.AddToRoleAsync(user, "Teacher");
            else await userManager.AddToRoleAsync(user, "Student");
            await signInManager.SignInAsync(user, isPersistent: false);
            Console.WriteLine("creation of account successful!");
            return RedirectToAction("Index", "Home");
        }
        Console.WriteLine("creation of account failed!");

        // foreach (var error in res.Errors)
        // {
        //     ModelState.AddModelError("", error.Description);
        //     Console.WriteLine(error.Description);
        // }
        foreach (var error in res.Errors)
        {
            Console.WriteLine(error.Description);
            if (error.Code.Contains("Password"))
                ModelState.AddModelError(nameof(model.Password), error.Description);
            else
                ModelState.AddModelError(string.Empty, error.Description);
        }
        Console.WriteLine("Errors");
        return View(model);
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