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
}
    
    


    
    
    
    








   
