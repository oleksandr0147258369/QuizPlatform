using System.Net;
using System.Net.Mail;
using Quizzy.Interfaces;

namespace Quizzy.Services;

public class SMTPService : ISMTPService
{
    public bool SendEmail(string email, string code, string name)
    {
        var fromAddress = new MailAddress("quizzytests@gmail.com", "Quizzy");
        var toAddress = new MailAddress(email, "Dear " + name);
        const string fromPassword = "eplc imyv edac hxzn";
        const string subject = "Verification code";
        string body = $"Hello dear {name}, here is your code - {code}, copy it and paste it on our website";

        var smtp = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
        };

        using var message = new MailMessage(fromAddress, toAddress)
        {
            Subject = subject,
            Body = body
        };

        try
        {
            smtp.Send(message);
            Console.WriteLine("Email sent successfully.");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            return false;
        }
    }
}