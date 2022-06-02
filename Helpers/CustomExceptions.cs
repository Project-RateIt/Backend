using Microsoft.Extensions.Primitives;

namespace rateit.Helpers;

[Serializable]
public class UserIsNotExist : Exception
{ 
    public UserIsNotExist() {}
}