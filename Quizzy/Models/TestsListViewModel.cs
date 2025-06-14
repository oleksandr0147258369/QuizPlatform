using Quizzy.Data.Entities;

namespace Quizzy.Models;

public class TestsListViewModel
{
    public List<Test> Tests { get; set; }
    public int CurrentPage { get; set; }
    public int TotalPages { get; set; }
    public string Error  { get; set; }
    public bool Show {get; set;}
}