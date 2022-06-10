namespace rateit.Interfaces;

public interface IGetObject
{
    Task<Models.User> GetUser(int id);
    Task<Models.Product> GetProduct(int id, int userId);
}