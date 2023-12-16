using System.Net;
using System.Net.Mail;

namespace rateit.Services.EmailService;

public class EmailService : IEmailService
{
    public async Task SendActivateEmail(string to, string username,  string code)
    {
        using var client = new SmtpClient();
        var body = await File.ReadAllTextAsync("EmailBody.html");

        body = body.Replace("{USERNAME}", username);
        body = body.Replace("{CODE}", code);
        body = body.Replace("{EMAIL}", to);
            
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("rzejan@gmail.com", "dzqp zxvb cddm laxi");
        using (var message = new MailMessage(
                   from: new MailAddress("rzejan@gmail.com", "noreply RateIt"),
                   to: new MailAddress(to, username)
               ))
        {

            message.Subject = "Aktywacja Konta - RateIt";
            message.Body = body;
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }

    public async Task SendResetPasswordEmail(string to, string username, string code)
    {
        using var client = new SmtpClient();
        var body = await File.ReadAllTextAsync("ResetPasswordEmailBody.html");

        body = body.Replace("{USERNAME}", username);
        body = body.Replace("{EMAIL}", to);
        body = body.Replace("{CODE}", code);
            
        client.Host = "smtp.gmail.com";
        client.Port = 587;
        client.DeliveryMethod = SmtpDeliveryMethod.Network;
        client.UseDefaultCredentials = false;
        client.EnableSsl = true;
        client.Credentials = new NetworkCredential("rzejan@gmail.com", "dzqp zxvb cddm laxi");
        using (var message = new MailMessage(
                   from: new MailAddress("rzejan@gmail.com", "noreply RateIt"),
                   to: new MailAddress(to, username)
               ))
        {

            message.Subject = "Reset Hasła - RateIt";
            message.Body = body;
            message.IsBodyHtml = true;

            await client.SendMailAsync(message);
        }
    }
}