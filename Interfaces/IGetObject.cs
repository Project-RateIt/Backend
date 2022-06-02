namespace rateit;

public interface IGetObject
{
    Task<Models.User> GetUser(int id);
}