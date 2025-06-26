namespace Quizzy.Models.Tests
{
    public class MyHomeworkViewModel
    {
        public int TestHomeworkId { get; set; }

        public int TestId { get; set; }
        public string TestName { get; set; }
        
        public bool IsPublished { get; set; }

        public DateTime CreatedUtc { get; set; }

        public bool HasDeadline { get; set; }
        public DateTime? Deadline { get; set; }

        public bool HasTimeToComplete { get; set; }
        public TimeSpan? TimeToComplete { get; set; }
        public string HomeworkCreatedByFirstName { get; set; }
        public string HomeworkCreatedByLastName { get; set; }

        public string HomeworkCreatedByFullName => $"{HomeworkCreatedByFirstName} {HomeworkCreatedByLastName}";
        public List<ResultCardViewModel> Results { get; set; } = new List<ResultCardViewModel>();
    }
}
