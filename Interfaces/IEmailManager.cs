namespace rateit.Interfaces;

public interface IEmailManager
{
    public Task SendEmail(string email);
}