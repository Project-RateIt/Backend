namespace rateit.Exceptions;

public abstract class BaseAppException : Exception
{
    public abstract int StatusCodeToRise { get; }
    public Dictionary<string,string[]> Errors { get; init; }
    public BaseAppException(string message) : base(message)
    {
        Errors = new Dictionary<string, string[]> { { "Message", new string[] { message } } };
    }
    public BaseAppException(Dictionary<string, string[]> errors)
    {
        Errors = errors;
    }
}