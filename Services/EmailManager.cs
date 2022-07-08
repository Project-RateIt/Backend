using System.Net;
using System.Net.Mail;
using rateit.Interfaces;

namespace rateit.Services;


public class EmailManager : IEmailManager
{

    public async Task SendEmailActivate(string email)
    {
        using MailMessage msg = new();
        msg.From = new MailAddress("eparafiadev@gmail.com");
        msg.To.Add(email);
        msg.Subject = "RateIt - Aktywacja konta";
        msg.Body = "Tu body";
        msg.Priority = MailPriority.High;
        msg.IsBodyHtml = true;

        using SmtpClient client = new ();
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("eparafiadev@gmail.com", "g8vYZOna8j231H7"); 
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network; 

        await client.SendMailAsync(msg);
    }
    
    public async Task SendEmailResetPassword(string email, string resetPassKey)
    {
        using MailMessage msg = new();
        msg.From = new MailAddress("eparafiadev@gmail.com");
        msg.To.Add(email);
        msg.Subject = "RateIt - Reset Hasła";
        msg.Body = $"<a href='http://91.227.2.183:443/user/setNewPassword?key={resetPassKey}'>Resetuj</a>";
        msg.Priority = MailPriority.High;
        msg.IsBodyHtml = true;

        using SmtpClient client = new ();
        client.EnableSsl = true;
        client.UseDefaultCredentials = false;
        client.Credentials = new NetworkCredential("eparafiadev@gmail.com", "g8vYZOna8j231H7"); 
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network; 

        await client.SendMailAsync(msg);    
    }
}