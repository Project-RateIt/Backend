using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace rateit.Helpers;

public class GetObject : IGetObject
{
    private readonly ISqlManager _sqlManager;
    private readonly ILogger<GetObject> _logger;

    public GetObject(ISqlManager sqlManager, ILogger<GetObject> logger)
    {
        _sqlManager = sqlManager;
        _logger = logger;
    }

    public async Task<Models.User> GetUser(int id)
    {
        var date = await _sqlManager.Reader($"SELECT * FROM users.user WHERE id = {id} AND isactivated = true;");

        Models.User user = new Models.User(date[0]["id"], date[0]["name"], date[0]["surname"], date[0]["email"], true);
        
        return user;
    }
}