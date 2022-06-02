using rateit.Helpers;

namespace rateit.User;

public class IsUserExistRequestModel : Request
{
    public IsUserExistRequestModel(int id, string token) : base(token)
    {
        Id = id;
    }
    public int Id {get;}
}