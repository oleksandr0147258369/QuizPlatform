using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Quizzy.Models;
using Quizzy.Models.Tests;

namespace Quizzy.Controllers;

public class TestsController : Controller
{
    private static ApplicationDbContext _db;

    public TestsController(ApplicationDbContext dbContext)
    {
        _db = dbContext;
    }

    public IActionResult Search_Test()
    {
        TestsListViewModel model = new TestsListViewModel
        {
            Show = false
        };
        return View(model);
    }


    [HttpPost]

    public async Task<IActionResult> Search(string query, string subject, int? classNumber)
    {
        int pageSize = 20;

        IQueryable<Test> results = _db.Tests
            .Include(t => t.Subject)
            .Include(t => t.Grade);



        if (!string.IsNullOrEmpty(query))
        {
            results = results.Where(t => t.Name.ToLower().Contains(query.ToLower())
                                         || (t.Description != null &&
                                             t.Description.ToLower().Contains(query.ToLower())));
        }

        if (!string.IsNullOrEmpty(subject))
        {
            results = results.Where(t => t.Subject.Name == subject);
        }

        if (classNumber.HasValue)
        {
            results = results.Where(t => t.Grade.Number == classNumber.Value);
        }

        TestsListViewModel m = new TestsListViewModel
        {
            Error = "No tests found",
            Show = true
        };

        if (!results.Any())
        {
            return View("Search_Test", m);
        }

        m = new TestsListViewModel
        {
            Tests = results.ToList(),
            CurrentPage = 1,
            TotalPages = (int)Math.Ceiling(results.Count() / (double)results.Count()),
            Error = "",
            Show = true
        };


        return View("Search_Test", m);
    }

    // public async Task<IActionResult> SearchPagination(int page = 1)
    // {
    //     int pageSize = 20;
    //
    //     var tests = await _db.Tests
    //         .OrderBy(t => t.TestId)
    //         .Skip((page - 1) * pageSize)
    //         .Take(pageSize)
    //         .ToListAsync();
    //
    //     int totalTests = await _db.Tests.CountAsync();
    //
    //     var viewModel = new TestsListViewModel
    //     {
    //         Tests = tests,
    //         CurrentPage = page,
    //         TotalPages = (int)Math.Ceiling(totalTests / (double)pageSize)
    //     };
    //
    //     return View("~/Views/Tests/Search_Test.cshtml",viewModel);
    // }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        return RedirectToAction();
    }

}