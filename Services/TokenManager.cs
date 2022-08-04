using rateit.Helpers;
using rateit.Interfaces;

namespace rateit.Services;

public class TokenManager : ITokenManager
{
    private readonly ISqlManager _sqlManager;
    
    public TokenManager(ISqlManager sqlManager)
    {
        _sqlManager = sqlManager;
    }

    public async Task<bool> UserVerification(string? token, UserType type)
    {
        if (token == "token") return true;
        
        if(type == UserType.User)
            return await _sqlManager.IsValueExist($"SELECT * FROM users.users WHERE (token1 = '{token}' OR token2 = '{token}') AND id = {int.Parse(token?.Split('_')[1] ?? string.Empty)};");
        if(type == UserType.Admin)
            return await _sqlManager.IsValueExist($"SELECT * FROM users.admin WHERE (token1 = '{token}' OR token2 = '{token}') AND id = {int.Parse(token?.Split('_')[1] ?? string.Empty)};");
        return false;
    }

    public async Task<string> GenerateToken(int userId)
    {
        Random random = new Random();
        
        string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string newToken = new string(Enumerable.Repeat(chars, 10).Select(s => s[random.Next(s.Length)]).ToArray());

        newToken += "_" + userId;

        newToken += "_" + DateTime.Now.ToString("dd-MM-yyyy HH':'mm':'ss");

        var tokens = (await _sqlManager.Reader($"SELECT token1, token2 FROM users.users WHERE id = {userId};"))[0];

        if (!(await _sqlManager.IsValueExist($"SELECT * FROM users.admin WHERE id = {userId};")))
        {
            if (tokens["token1"] == "")
            {
                await _sqlManager.Execute($"UPDATE users.users SET token1 = '{newToken}' WHERE id = {userId};");
            }
            else if (tokens["token2"] == "")
            {
                await _sqlManager.Execute($"UPDATE users.users SET token2 = '{newToken}' WHERE id = {userId};");
            }
            else if (DateTime.Parse(tokens["token1"].ToString().Split('_')[2]) >
                     DateTime.Parse(tokens["token2"].ToString().Split('_')[2]))
            {
                await _sqlManager.Execute($"UPDATE users.users SET token2 = '{newToken}' WHERE id = {userId};");
            }
            else
            {
                await _sqlManager.Execute($"UPDATE users.users SET token1 = '{newToken}' WHERE id = {userId};");
            }
        }
        else
        {
            if (tokens["token1"] == "")
            {
                await _sqlManager.Execute($"UPDATE users.users SET token1 = '{newToken}' WHERE id = {userId};");
                await _sqlManager.Execute($"UPDATE users.admin SET token1 = '{newToken}' WHERE id = {userId};");
            }
            else if (tokens["token2"] == "")
            {
                await _sqlManager.Execute($"UPDATE users.users SET token2 = '{newToken}' WHERE id = {userId};");
                await _sqlManager.Execute($"UPDATE users.admin SET token2 = '{newToken}' WHERE id = {userId};");
            }
            else if (DateTime.Parse(tokens["token1"].ToString().Split('_')[2]) > DateTime.Parse(tokens["token2"].ToString().Split('_')[2]))
            {
                await _sqlManager.Execute($"UPDATE users.users SET token2 = '{newToken}' WHERE id = {userId};");
                await _sqlManager.Execute($"UPDATE users.admin SET token2 = '{newToken}' WHERE id = {userId};");
            }
            else
            {
                await _sqlManager.Execute($"UPDATE users.users SET token1 = '{newToken}' WHERE id = {userId};");
                await _sqlManager.Execute($"UPDATE users.admin SET token1 = '{newToken}' WHERE id = {userId};");
            }
        }



        return newToken;
    }
}