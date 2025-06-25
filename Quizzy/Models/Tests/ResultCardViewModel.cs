namespace Quizzy.Models.Tests
{
    public class ResultCardViewModel
    {
        public int ResultId { get; set; }
        public int UserId { get; set; }        // Додано UserId
        public int TestId { get; set; }        // Додано TestId

        public string FullName { get; set; }
        public int Points { get; set; }
        public int TotalPoints { get; set; }

        public int SuccessPercentage => TotalPoints == 0 ? 0 : (int)((double)Points / TotalPoints * 100);

        public TimeSpan TimeSpent { get; set; }         // Час, витрачений на тест
        public TimeSpan TimePerQuestion { get; set; }
    }

}
