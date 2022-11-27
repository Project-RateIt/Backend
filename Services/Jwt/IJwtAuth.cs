namespace rateit.Jwt;

public interface IJwtAuth
{
    public Task<GeneratedToken> GenerateJwt(Guid id, string role);
    public Guid GetCurrentUser(HttpContext context);
}