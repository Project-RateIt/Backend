namespace rateit.Models;

[Serializable]
public class User
{
    public int Id { get; }
    public string Name { get; }
    public string Surname { get; }
    public string Email { get; }

    public User(int id, string name, string surname, string email)
    {
        Id = id;
        Name = name;
        Surname = surname;
        Email = email;
    }
}