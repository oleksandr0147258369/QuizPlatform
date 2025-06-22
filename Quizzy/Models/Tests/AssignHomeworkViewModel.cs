using System;
using System.ComponentModel.DataAnnotations;

public class AssignHomeworkViewModel
{
    [Required]
    public string TestName { get; set; }  // якщо потрібно передавати назву тесту

    [Required]
    public DateTime DeadlineDate { get; set; }  // дата дедлайну

    [Required]
    public TimeSpan DeadlineTime { get; set; }  // час дедлайну

    

    public bool LimitTime { get; set; }

    public int? TimeLimitMinutes { get; set; }

    public int TestId { get; set; } // має бути або приховане поле або з бекенду
}

