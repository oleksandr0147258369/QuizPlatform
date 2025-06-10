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
        return View();
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

}









    


    
    
    
    








   
