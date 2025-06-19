using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Quizzy.Data;
using Quizzy.Data.Entities;
using Quizzy.Data.Entities.Identity;
using Quizzy.Models;
using Quizzy.Models.Tests;

namespace Quizzy.Controllers;

public class TestsController(UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    IMapper mapper, ApplicationDbContext _db) : Controller
{

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
        if(User.Identity.IsAuthenticated && User.IsInRole("teacher"))
            return View();
        return RedirectToAction("Login", "Account");
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        
        var test = mapper.Map<Test>(model);
        var userIdStr = userManager.GetUserId(User);
        test.CreatedById = Convert.ToInt32(userIdStr);
        var grade = _db.Grades.FirstOrDefault(g => g.Number.ToString() == model.Grade).GradeId;
        if (grade == null) return BadRequest("Invalid grade");

        test.GradeId = grade;
        var subject = await _db.Subjects.FirstOrDefaultAsync(g => g.Name == model.Subject);
        if (subject == null) return BadRequest("Invalid subject");

        test.SubjectId = subject.SubjectId;
        _db.Tests.Add(test);
        await _db.SaveChangesAsync();
        return RedirectToAction("Builder", "Tests", new { id = test.TestId });
    }

    [HttpGet("Tests/Edit/{id}")]
    public IActionResult Edit(int id)
    {
        if (!User.Identity.IsAuthenticated || !User.IsInRole("teacher"))
        {
            return RedirectToAction("Login", "Account");
        }

        var test = _db.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(t => t.TestId == id);
        if (test == null)
        {
            return RedirectToAction("Create", "Tests");
        }
        var userId = int.Parse(userManager.GetUserId(User));
        if (test.CreatedById != userId)
        {
            return RedirectToAction("Login", "Account");
        }
        var model = mapper.Map<EditViewModel>(test);
        model.Subject = _db.Subjects.First(a => a.SubjectId == test.SubjectId).Name;
        model.Grade = _db.Grades.First(a => a.GradeId == test.GradeId).Number.ToString();
        Console.WriteLine(model.Subject);
        return View(model);
    }

    [HttpPost("Tests/Edit/{id}")]
    public async Task<IActionResult> Edit(EditViewModel model, int id)
    {
        var test = _db.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(t => t.TestId == id);
        if (test == null)
        {
            Console.WriteLine("Test not found");
            return View("Create");
        }
        if (!ModelState.IsValid)
        {
            return View(model);
        }
        mapper.Map(model, test);
        
        var grade = _db.Grades.FirstOrDefault(g => g.Number.ToString() == model.Grade).GradeId;
        if (grade == null) return BadRequest("Invalid grade");

        test.GradeId = grade;
        var subject = await _db.Subjects.FirstOrDefaultAsync(g => g.Name == model.Subject);
        if (subject == null) return BadRequest("Invalid subject");

        test.SubjectId = subject.SubjectId;
        
        _db.Tests.Update(test);
        await _db.SaveChangesAsync();
        return RedirectToAction("Builder", "Tests", new { id = test.TestId });
    }

    [HttpGet("Tests/AddQuestion/{id}")]
    public IActionResult AddQuestion(int id)
    {
        if (!User.Identity.IsAuthenticated || !User.IsInRole("teacher"))
        {
            return RedirectToAction("Login", "Account");
        }

        var test = _db.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(t => t.TestId == id);
        if (test == null)
        {
            return RedirectToAction("Create", "Tests");
        }
        var userId = int.Parse(userManager.GetUserId(User));
        if (test.CreatedById != userId)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new AddQuestionViewModel
        {
            TestId = id,
            Answers = Enumerable.Range(0, 8).Select(_ => new AnswerViewModel()).ToList()
        };

        return View(model);
    }

    [HttpPost("Tests/AddQuestion/{id}")]
    public async Task<IActionResult> AddQuestion(AddQuestionViewModel model, int id)
    {
        if (!User.Identity.IsAuthenticated || !User.IsInRole("teacher"))
        {
            return RedirectToAction("Login", "Account");
        }

        var test = _db.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(t => t.TestId == id);
        if (test == null)
        {
            return RedirectToAction("Create", "Tests");
        }
        var userId = int.Parse(userManager.GetUserId(User));
        if (test.CreatedById != userId)
        {
            return RedirectToAction("Login", "Account");
        }
        
        if (!ModelState.IsValid)
        {
            model.Answers ??= Enumerable.Range(0, 8).Select(_ => new AnswerViewModel()).ToList();
            return View(model);
        }

        var filteredAnswers = model.Answers.Where(a => !string.IsNullOrWhiteSpace(a.Text)).ToList();
        if (filteredAnswers.Count < 2)
        {
            ModelState.AddModelError("", "Please provide at least 2 answers.");
            return View(model);
        }

        if (!filteredAnswers.Any(a => a.IsCorrect))
        {
            ModelState.AddModelError("", "Please provide at least one correct.");
            return View(model);
        }
        var correctAnswersCount = filteredAnswers.Count(a => a.IsCorrect);

        if (!model.HasMultipleCorrect && correctAnswersCount > 1)
        {
            ModelState.AddModelError("", "Only one answer can be correct if checkbox is unchecked.");
            return View(model);
        }

        var question = new Question
        {
            TestId = id,
            Text = model.Text,
            Points = model.Points,
            HasMultipleCorrect = model.HasMultipleCorrect,
            Answers = filteredAnswers.Select(a => new Answer()
            {
                IsCorrect = a.IsCorrect,
                Text = a.Text
            }).ToList()
        };
        _db.Questions.Add(question);
        await _db.SaveChangesAsync();
        return RedirectToAction("Builder", "Tests", new { id });
    }

    [HttpGet("Tests/Builder/{id}")]
    public IActionResult Builder(int id)
    {
        if (!User.Identity.IsAuthenticated || !User.IsInRole("teacher"))
        {
            return RedirectToAction("Login", "Account");
        }

        var test = _db.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(t => t.TestId == id);
        if (test == null)
        {
            return RedirectToAction("Create", "Tests");
        }
        var userId = int.Parse(userManager.GetUserId(User));
        if (test.CreatedById != userId)
        {
            return RedirectToAction("Login", "Account");
        }

        var model = new BuilderViewModel
        {
            IsPrivate = test.IsPrivate,
            Name = test.Name,
            Questions = test.Questions.Select(a => new BuilderQuestionViewModel
            {
                Id = a.QuestionId,
                Text = a.Text,
                HasMultipleCorrect = a.HasMultipleCorrect,
                Points = a.Points,
                Answers = a.Answers.Select(an => new AnswerViewModel
                {
                    IsCorrect = an.IsCorrect,
                    Text = an.Text
                }).ToList()
            }).ToList()
        };
        return View(model);
    }

    [HttpGet("Tests/EditQuestion/{questionId}")]
    public IActionResult EditQuestion(int questionId)
    {
        Question question = _db.Questions.Include(q => q.Answers).First(q => q.QuestionId == questionId);
        if (question.Text == string.Empty)
        {
            return RedirectToAction("Builder", "Tests", new { id = question.TestId });
        }

        var model = new EditQuestionViewModel
        {
            TestId = question.TestId,
            Text = question.Text,
            Points = question.Points,
            HasMultipleCorrect = question.HasMultipleCorrect,
            Answers = question.Answers.Select(an => new AnswerViewModel
            {
                IsCorrect = an.IsCorrect,
                Text = an.Text
            }).ToList()
        };
        if (model.Answers.Count < 8)
        {
            model.Answers.AddRange(Enumerable.Range(0, 8 - model.Answers.Count).Select(_ => new AnswerViewModel()));
        }

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> EditQuestion(EditQuestionViewModel model)
    {
        if (!User.Identity.IsAuthenticated || !User.IsInRole("teacher"))
        {
            return RedirectToAction("Login", "Account");
        }
        if (!ModelState.IsValid)
        {
            model.Answers ??= Enumerable.Range(0, 8).Select(_ => new AnswerViewModel()).ToList();
            return View(model);
        }

        var filteredAnswers = model.Answers.Where(a => !string.IsNullOrWhiteSpace(a.Text)).ToList();
        if (filteredAnswers.Count < 2)
        {
            ModelState.AddModelError("", "Please provide at least 2 answers.");
            return View(model);
        }

        if (!filteredAnswers.Any(a => a.IsCorrect))
        {
            ModelState.AddModelError("", "Please provide at least one correct.");
            return View(model);
        }
        var correctAnswersCount = filteredAnswers.Count(a => a.IsCorrect);

        if (!model.HasMultipleCorrect && correctAnswersCount > 1)
        {
            ModelState.AddModelError("", "Only one answer can be correct if checkbox is unchecked.");
            return View(model);
        }
        var question = _db.Questions
            .Include(q => q.Answers)
            .FirstOrDefault(q => q.QuestionId == model.QuestionId);

        if (question == null)
        {
            return NotFound();
        }

        question.Text = model.Text;
        question.Points = model.Points;
        question.HasMultipleCorrect = model.HasMultipleCorrect;

        _db.Answers.RemoveRange(question.Answers);

        question.Answers = filteredAnswers.Select(a => new Answer
        {
            Text = a.Text,
            IsCorrect = a.IsCorrect
        }).ToList();

        await _db.SaveChangesAsync();

        return RedirectToAction("Builder", "Tests", new { id = question.TestId });
    }

    [HttpGet("Tests/DeleteQuestion/{questionId}")]
    public IActionResult DeleteQuestion(int questionId, int testId)
    {
        var question = _db.Questions.Include(q => q.Test).FirstOrDefault(q => q.QuestionId == questionId);
        if (question != null && question.Test.CreatedById.ToString() == userManager.GetUserId(User))
        {
            _db.Questions.Remove(question);
            _db.SaveChanges();
        }
        return RedirectToAction("Builder", new { id = testId });
    }
    [HttpGet("Tests/DuplicateQuestion/{questionId}")]
    public IActionResult DuplicateQuestion(int questionId)
    {
        var original = _db.Questions.Include(q => q.Answers).FirstOrDefault(q => q.QuestionId == questionId);
        if (original == null) return RedirectToAction("Builder", new { id = original.TestId });

        var duplicate = new Question
        {
            Text = original.Text + " (copy)",
            HasMultipleCorrect = original.HasMultipleCorrect,
            Points = original.Points,
            TestId = original.TestId,
            Answers = original.Answers.Select(a => new Answer
            {
                Text = a.Text,
                IsCorrect = a.IsCorrect
            }).ToList()
        };

        _db.Questions.Add(duplicate);
        _db.SaveChanges();
        return RedirectToAction("Builder", new { id = original.TestId });
    }

    [HttpPost]
    public IActionResult DeleteTest(int id)
    {
        var test = _db.Tests.Include(t => t.Questions).ThenInclude(q => q.Answers).FirstOrDefault(t => t.TestId == id);
        var userId = int.Parse(userManager.GetUserId(User));

        if (test == null || test.CreatedById != userId)
        {
            return RedirectToAction("Builder", new { id });
        }

        _db.Tests.Remove(test);
        _db.SaveChanges();

        return RedirectToAction("MyTests");
    }
}