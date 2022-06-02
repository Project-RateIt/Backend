using rateit.Helpers;

namespace rateit.User;

public class UserRegisterRequestModel
{
    public UserRegisterRequestModel(string name, string surname, string email, string password)
    {
        Name = name;
        Surname = surname;
        Email = email;
        Password = password;
    }

    public string Name {get;}
    public string Surname {get;}
    public string Email {get;}
    public string Password {get;}
}