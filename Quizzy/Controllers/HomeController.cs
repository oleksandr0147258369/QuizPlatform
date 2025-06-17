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
<<<<<<< Updated upstream
}
    
    


    
    
    
    
=======

    public IActionResult Search_Tests()
    {
        return View();
    }

    public IActionResult Test()
    {
        return View();
    }
    //public IActionResult Search(string query, string subject, int? classNumber)
    //{
    //    var results = _db.Tests
    //        .Include(t => t.Subject)
    //        .AsQueryable();
    //    foreach (var item in results)
    //    {
    //        item.GradeId = item.GradeId switch
    //        {
    //            1 => 5,
    //            2 => 6,
    //            3 => 7,
    //            4 => 8,
    //            5 => 9,
    //            6 => 10,
    //            7 => 11,

    //        };


    //    }
    //    if (!string.IsNullOrEmpty(query))
    //    {
    //        results = results.Where(t => t.Name.Contains(query));
    //    }
    //    if (!string.IsNullOrEmpty(subject))
    //    {
    //        results = results.Where(t => t.Subject.Name == subject);
    //    }
    //    //if (classNumber.HasValue)
    //    //{
    //    //    results = results.Where(t => t.GradeId == classNumber.Value);
    //    //}

    //    return View("Search_Tests", results.ToList());
    //}
    //public IActionResult Search(string query, string subject, int? classNumber)
    //{
    //    var results = _db.Tests
    //        .Include(t => t.Subject)
    //        .Include(t => t.Grade)
    //        .AsQueryable();

    //    if (!string.IsNullOrEmpty(query))
    //    {
    //        results = results.Where(t => t.Name.Contains(query));
    //    }

    //    if (!string.IsNullOrEmpty(subject))
    //    {
    //        results = results.Where(t => t.Subject.Name == subject);
    //    }

    //    if (classNumber.HasValue)
    //    {
    //        results = results.Where(t => t.Grade.Number == classNumber.Value);
    //    }

    //    return View("Search_Tests", results.ToList());
    //}

    public IActionResult Search(string query, string subject, int? classNumber)
    {
        var results = _db.Tests
            .Include(t => t.Subject)
            .Include(t => t.Grade)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
        {
            var keywords = query
                .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Select(k => k.Trim().ToLower());

            results = results.Where(t =>
                keywords.All(k => t.Name.ToLower().Contains(k))
            );
        }

        if (!string.IsNullOrEmpty(subject))
        {
            results = results.Where(t => t.Subject.Name == subject);
        }

        if (classNumber.HasValue)
        {
            results = results.Where(t => t.Grade.Number == classNumber.Value);
        }

        return View("Search_Tests", results.ToList());
    }





    //[HttpGet]
    //public IActionResult Show_Test(int id)
    //{
    //    var test = _db.Tests
    //        .Include(t => t.Name)
    //        .Include(t => t.Subject) 
    //        .Include(t => t.Description)
    //        .Include(t => t.CreatedById)
    //        .Include(t => t.CreatedUtc)
    //        .Include(t => t.SubjectId)
    //        .Include(t => t.GradeId)
    //        .Include(t => t.QuestionsQuantity)
    //        .FirstOrDefault(t => t.TestId == id);

    //    if (test == null)
    //        return NotFound();

    //    return View(test);
    //}


}









>>>>>>> Stashed changes
















