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

public class RunController(
    UserManager<UserEntity> userManager,
    SignInManager<UserEntity> signInManager,
    IMapper mapper,
    ApplicationDbContext _db) : Controller
{


    public IActionResult Code(string code)
    {
        var model = new RunTestViewModel
        {
            IsSuccessful = true,
            FullView = string.IsNullOrEmpty(code),
            Code = code
        };
        if (code == null) return View(model);


        if (!int.TryParse(code, out int testId))
            return BadRequest("Invalid code format.");

        var testExists = _db.Tests.Any(t => t.TestId == testId);
        if (!testExists)
            return NotFound("Test not found.");

        return View(model);
    }

    [HttpGet]
    public IActionResult HomeworkCode(string hwcode)
    {
        if (hwcode == null) return View(new RunHomeworkTestViewModel() { FullView = true });

        var hw = _db.TestHomeworks.FirstOrDefault(t => t.TestHomeworkId.ToString() == hwcode);
        if (hw == null) return NotFound("Test homework not found.");
        var code = hw.TestId;
        var model = new RunHomeworkTestViewModel()
        {
            IsSuccessful = true,
            FullView = string.IsNullOrEmpty(hwcode),
            HomeworkCode = hwcode,
            Code = code.ToString()
        };
        if (code == null || string.IsNullOrEmpty(hwcode)) return View(model);



        var testExists = _db.Tests.Any(t => t.TestId == code);
        if (!testExists)
            return NotFound("Test not found.");

        return View(model);
    }



    public IActionResult CodeLink()
    {
        return View();
    }
    
    

    [HttpGet]
    public async Task<IActionResult> TestRun()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> HomeworkRun()
    {
        
        return View();
    }


    public IActionResult ShowResult()
    {

        var viewModel = new ResultViewModel
        {
            Points = int.Parse(TempData["Points"].ToString()),
            TimeSpent = TimeSpan.Parse(TempData["TimeSpent"].ToString()),
            TimePerQuestion = TimeSpan.Parse(TempData["TimePerQuestion"].ToString()),
            StartedAt = DateTime.Parse(TempData["StartedAt"].ToString()),
            FinishedAt = DateTime.Parse(TempData["FinishedAt"].ToString()),
            UserName = TempData["UserName"].ToString(),
            MaxPoints = int.Parse(TempData["MaxPoints"].ToString())
        };
        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> ShowResult(int id)
    {
        var session = await _db.TestSessions
            .FirstOrDefaultAsync(s => s.TestSessionId == id);

        if (session == null)
            return NotFound("Session not found");

        var result = await _db.Results
            .FirstOrDefaultAsync(r => r.TestSessionId == id);

        if (result == null)
            return NotFound("Result not found");

        var model = new ResultViewModel
        {
            FinishedAt = session.FinishedAt,
            Points = result.Points,
            TimeSpent = result.TimeSpent,
            TimePerQuestion = result.TimePerQuestion,
            StartedAt = session.StartedAt,
            UserName = session.Name,
            MaxPoints = result.TotalPoints
        };

        return View("ShowResult", model);
    }








    [HttpPost]
    public async Task<IActionResult> Code(string name, string code)
    {
        if (!int.TryParse(code, out int testId))
            return BadRequest("Invalid code format.");

        var test = await _db.Tests
            .Include(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.TestId == testId);

        if (test == null)
            return NotFound("Test not found.");

        // Спроба знайти вже існуючу незавершену сесію
        var userId = int.Parse(userManager.GetUserId(User));
        var existingSession = await _db.TestSessions
            .FirstOrDefaultAsync(s =>
                s.TestId == testId &&
                s.UserId == userId &&
                !s.IsFinished);

        int sessionId;

        if (existingSession != null)
        {
            sessionId = existingSession.TestSessionId;
        }
        else
        {
            var session = new TestSession
            {
                UserId = userId,
                Name = name,
                IsTestHomework = false,
                TestId = testId,
                StartedAt = DateTime.UtcNow,
                IsFinished = false,
                Result = null
            };

            await _db.TestSessions.AddAsync(session);
            await _db.SaveChangesAsync();
            sessionId = session.TestSessionId;
        }

        RunTestViewModel model = new RunTestViewModel
        {
            Questions = test.Questions.ToList(),
            Code = code,
            IsSuccessful = true,
            SessionId = sessionId
        };

        return View("TestRun", model);
    }


    [HttpPost]
    public async Task<IActionResult> HomeworkCode(RunHomeworkTestViewModel model)
    {
        if (!int.TryParse(model.HomeworkCode, out int testHwId))
            return BadRequest("Invalid hw code format.");

        var hw = await _db.TestHomeworks.FirstOrDefaultAsync(t => t.TestHomeworkId == testHwId);
        if (hw == null) return NotFound("Test homework not found.");
        if(hw.Deadline < DateTime.UtcNow)
            return NotFound("Deadline has expired.");
        var test = await _db.Tests
            .Include(e => e.Questions)
            .ThenInclude(a => a.Answers)
            .FirstOrDefaultAsync(t => t.TestId == hw.TestId);
        if (test == null) return NotFound("Test not found.");

        var userId = int.Parse(userManager.GetUserId(User));
        var existingSession = await _db.TestSessions
            .FirstOrDefaultAsync(s => s.TestId == test.TestId && s.UserId == userId && !s.IsFinished);

        int sessionId;
        DateTime startTime;

        if (existingSession != null)
        {
            // Якщо сесія активна — продовжуємо з тим самим часом
            sessionId = existingSession.TestSessionId;
            startTime = existingSession.StartedAt;
        }
        else
        {
            // Якщо сесія завершена або не існує — створюємо нову
            var session = new TestSession
            {
                UserId = userId,
                Name = model.Username,
                IsTestHomework = true,
                TestHomeworkId = hw.TestHomeworkId,
                TestId = test.TestId,
                StartedAt = DateTime.UtcNow,
                IsFinished = false,
                Result = null
            };

            await _db.TestSessions.AddAsync(session);
            await _db.SaveChangesAsync();

            sessionId = session.TestSessionId;
            startTime = session.StartedAt;
        }

        var vm = new RunHomeworkTestViewModel
        {
            Questions = test.Questions.ToList(),
            Code = test.TestId.ToString(),
            HomeworkCode = hw.TestHomeworkId.ToString(),
            IsSuccessful = true,
            SessionId = sessionId,
        };
            

        return View("HomeworkRun", vm);
    }


    [HttpPost]
    public async Task<IActionResult> SubmitAnswer([FromBody] AnswerSubmission submission)
    {
        // submission.code, submission.questionIndex, submission.answerIds

        var session = await _db.TestSessions
            .FirstOrDefaultAsync(s => s.TestSessionId == submission.SesId);

        if (session == null) return BadRequest("Session not found");

        foreach (var answerId in submission.answerIds)
        {
            _db.UserAnswers.Add(new UserAnswer
            {
                AnswerId = int.Parse(answerId),

                TestSessionId = session.TestSessionId
            });
        }

        await _db.SaveChangesAsync();
        return Ok();
    }

    public class AnswerSubmission
    {
        public string code { get; set; }
        public int questionIndex { get; set; }
        public List<string> answerIds { get; set; }
        public int SesId { get; set; }
    }

    [HttpPost]
    public async Task<IActionResult> GetQuestion([FromBody] QuestionRequestModel model)
    {
        if (!int.TryParse(model.Code, out int testId))
            return BadRequest("Invalid code");

        var test = await _db.Tests
            .Include(t => t.Questions.OrderBy(q => q.QuestionId))
            .ThenInclude(q => q.Answers)
            .FirstOrDefaultAsync(t => t.TestId == testId);

        if (test == null)
            return BadRequest("Test not found");

        if (model.Index < 0 || model.Index >= test.Questions.Count)
            return BadRequest("Index out of range");

        var question = test.Questions.ElementAt(model.Index);

        return Json(new
        {
            text = question.Text,
            hasMultipleCorrect = question.HasMultipleCorrect,
            answers = question.Answers.Select(a => new { a.AnswerId, a.Text })
        });
    }

    [HttpPost]
    public async Task<IActionResult> Result([FromBody] SessionDto dto)
    {
        Console.WriteLine($"Result started for session id: {dto.SesId}");

        var userAnswers = _db.UserAnswers
            .Where(ua => ua.TestSessionId == dto.SesId)
            .Include(ua => ua.Answer)
            .ThenInclude(a => a.Question)
            .ToList();

        Console.WriteLine($"User answers count: {userAnswers.Count}");

        var groupedByQuestion = userAnswers
            .GroupBy(ua => ua.Answer?.QuestionId)
            .Where(g => g.Key != null)
            .ToList();

        Console.WriteLine($"Grouped questions count: {groupedByQuestion.Count}");

        int totalPoints = 0;

        foreach (var questionGroup in groupedByQuestion)
        {
            var questionId = questionGroup.Key;
            Console.WriteLine($"Processing question id: {questionId}");

            var userSelectedAnswerIds = questionGroup
                .Where(ua => ua.Answer != null)
                .Select(ua => ua.Answer.AnswerId)
                .ToHashSet();

            Console.WriteLine($"User selected answers: {string.Join(", ", userSelectedAnswerIds)}");

            var correctAnswerIds = _db.Answers
                .Where(a => a.QuestionId == questionId && a.IsCorrect)
                .Select(a => a.AnswerId)
                .ToHashSet();

            Console.WriteLine($"Correct answers: {string.Join(", ", correctAnswerIds)}");

            if (userSelectedAnswerIds.SetEquals(correctAnswerIds))
            {
                var firstAnswer = questionGroup.FirstOrDefault()?.Answer;
                if (firstAnswer == null)
                {
                    Console.WriteLine($"Warning: First answer is null for question id {questionId}");
                    continue;
                }

                if (firstAnswer.Question == null)
                {
                    Console.WriteLine($"Warning: Question is null for answer id {firstAnswer.AnswerId}");
                    continue;
                }

                var points = firstAnswer.Question.Points;
                Console.WriteLine($"Adding points: {points}");
                totalPoints += points;
            }
            else
            {
                Console.WriteLine("User answers do not match correct answers");
            }
        }

        Console.WriteLine($"ID - {dto.SesId}");
        var ThisSession = _db.TestSessions.FirstOrDefault(t => t.TestSessionId == dto.SesId);
        Console.WriteLine(ThisSession.StartedAt);

        Console.WriteLine("ksjhfkjsdfljsdlfjlsdjf lasdf");
        if (ThisSession == null)
        {
            Console.WriteLine("Error: Session not found");
            return BadRequest("Session not found");
        }

        ThisSession.IsFinished = true;
        ThisSession.FinishedAt = DateTime.UtcNow;
        _db.TestSessions.Update(ThisSession);
        await _db.SaveChangesAsync();

        Console.WriteLine("Session marked as finished");

        var testId = ThisSession.TestId;
        Console.WriteLine($"Test id from session: {testId}");

        int questionCount = _db.Questions
            .Where(q => q.TestId == testId)
            .Count();

        Console.WriteLine($"Question count for test: {questionCount}");

        Console.WriteLine(ThisSession.StartedAt);
        Console.WriteLine(ThisSession.FinishedAt);

        if (!ThisSession.FinishedAt.HasValue || ThisSession.StartedAt == null)
        {
            return BadRequest("Session has incomplete timing info");
        }


        var timeSpent = ThisSession.FinishedAt.Value - ThisSession.StartedAt;
        var timePerQuestion = questionCount > 0 ? timeSpent / questionCount : TimeSpan.Zero;

        int maxPoints = _db.Questions
            .Where(q => q.TestId == testId)
            .Sum(q => q.Points);


        Result res = new Result
        {
            TestSessionId = dto.SesId,
            Points = totalPoints,
            TimeSpent = timeSpent,
            TimePerQuestion = timePerQuestion,
            TotalPoints = maxPoints
        };


        await _db.Results.AddAsync(res);
        await _db.SaveChangesAsync();

        Console.WriteLine($"Result saved: Points={totalPoints}");



        ResultViewModel v = new ResultViewModel
        {
            FinishedAt = ThisSession.FinishedAt,
            Points = totalPoints,
            TimePerQuestion = res.TimePerQuestion,
            TimeSpent = res.TimeSpent,
            StartedAt = ThisSession.StartedAt,
            UserName = _db.TestSessions.FirstOrDefault(s => s.TestSessionId == dto.SesId).Name,
            MaxPoints = maxPoints
        };
        Console.WriteLine(v.UserName);

        TempData["Points"] = totalPoints;
        TempData["MaxPoints"] = maxPoints;
        TempData["TimeSpent"] = timeSpent.ToString();
        TempData["TimePerQuestion"] = timePerQuestion.ToString();
        TempData["StartedAt"] = ThisSession.StartedAt.ToString();
        TempData["FinishedAt"] = ThisSession.FinishedAt.ToString();
        TempData["UserName"] = _db.TestSessions.FirstOrDefault(s => s.TestSessionId == dto.SesId).Name;
        return RedirectToAction("ShowResult", new { id = dto.SesId });

    }
}

public class QuestionRequestModel
{
    public string Code { get; set; } = string.Empty;  // Ідентифікатор тесту (TestId у вигляді рядка)
    public int Index { get; set; }                    // Номер питання (індекс у списку)
}


public class SessionDto
{
    public int SesId { get; set; }
}
