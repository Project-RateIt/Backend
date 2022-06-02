using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace rateit.Helpers;

public class GetObject : IGetObject
{
    private const string DateFromat = "G";
    private readonly ISqlManager _sqlManager;
    private readonly ILogger<GetObject> _logger;

    public GetObject(ISqlManager sqlManager, ILogger<GetObject> logger)
    {
        _sqlManager = sqlManager;
        _logger = logger;
    }

    //public async Task<User> GetUser(int id)
    //{
    //    return user;
    //}
}