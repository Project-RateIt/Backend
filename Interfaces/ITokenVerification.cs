using rateit.Helpers;

namespace rateit;

public interface ITokenManager
{
    public Task<bool> UserVerification(string? token, UserType type);
    public Task<string> GenerateToken(int userId);
}