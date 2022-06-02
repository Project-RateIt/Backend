using rateit.Helpers;

namespace rateit.User;

public class LoginUserResponseModel
{
    public LoginUserResponseModel(Models.User user, string token)
    {
        User = user;
        Token = token;
    }
    
    public Models.User User {get;}
    public string Token {get;}
}