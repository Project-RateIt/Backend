namespace rateit.Models;

public class User
{
    public int Id;
    public string Name;
    public string Surname;
    public string Email;
    public bool IsActivated;

    public User(int id, string name, string surname, string email, bool isActivated)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
        IsActivated = isActivated;
    }
}