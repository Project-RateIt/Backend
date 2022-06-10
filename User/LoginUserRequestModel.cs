namespace rateit.User;

public class LoginUserRequestModel
{
    public LoginUserRequestModel(string email, string password)
    {
        Email = email;
        Password = password;
    }
    
    public string Email {get;}
    public string Password {get;}
}