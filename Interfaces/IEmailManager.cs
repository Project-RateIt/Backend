namespace rateit.Interfaces;

public interface IEmailManager
{
    public Task SendEmailActivate(string email);
    public Task SendEmailResetPassword(string email, string resetPassKey);
}