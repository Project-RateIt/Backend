using rateit.Helpers;

namespace rateit.Interfaces;

public interface ITokenManager
{
    public Task<bool> UserVerification(string? token, UserType type);
    public Task<string> GenerateToken(int userId);
}