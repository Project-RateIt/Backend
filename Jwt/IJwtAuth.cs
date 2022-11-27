namespace rateit.Jwt;

public interface IJwtAuth
{
    public Task<string> GenerateJwt(Guid id, string role);
    public Guid GetCurrentUser(HttpContext context);
}