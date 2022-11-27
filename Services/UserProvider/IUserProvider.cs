namespace rateit.Services;

public interface IUserProvider
{
    Guid Id { get; }
    void SetUser(Guid id);

}