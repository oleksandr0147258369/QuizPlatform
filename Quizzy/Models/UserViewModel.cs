using Quizzy.Data.Entities;

namespace Quizzy.Models;

public class UserViewModel
{
    public int Id { get; init; }
    public string FirstName { get; set; }
    public string? MiddleName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? About { get; set; }
    public string? SchoolName { get; set; }
    public string PhotoPath { get; set; }
    
    public string CurrentPassword { get; set; }
    
    public string? OldPassword { get; set; }
    public string? NewPassword { get; set; }

    public UserViewModel(User user)
    {
        FirstName = user.FirstName;
        MiddleName = user.MiddleName;
        LastName = user.LastName;
        Email = user.Email;
        PhoneNumber = user.PhoneNumber;
        SchoolName = user.School.Name;
        About = user.About;
        PhotoPath = user.PhotoPath;
        CurrentPassword = user.Password;

        Id = user.UserId;
    }
}