namespace rateit;

public interface IEmailManager
{
    public Task SendEmail(string email);
}