namespace rateit.Services.EmailService;

public interface IEmailService
{
    Task SendActivateEmail(string to, string username, string code);
    Task SendResetPasswordEmail(string to, string username, string code);
}