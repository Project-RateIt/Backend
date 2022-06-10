using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using rateit.Models;


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
        var date = await _sqlManager.Reader($"SELECT * FROM users.users WHERE id = {id};");

        if (date.Count == 0) throw new Exception("ErrGetUser");
        
        Models.User user = new Models.User(date[0]["id"], date[0]["name"], date[0]["surname"], date[0]["email"]);
        
        return user;
    }

    public async Task<Product> GetProduct(int id, int userId)
    {
        var data = await _sqlManager.Reader($"SELECT * FROM products.products WHERE id = {id};");

        if (data.Count == 0) throw new Exception("ErrGetProduct");
        var item = data[0];

        bool follow = await _sqlManager.IsValueExist($"SELECT * FROM user_details.my_product_{userId} WHERE productid = {id};");
        List<Dictionary<string, dynamic>> rateData = await _sqlManager.Reader($"SELECT * FROM user_details.my_product_{userId} WHERE productid = {id};");

        bool rated = rateData.Count > 0;
        int myRate = 0;
        if (rated)
        {
            myRate = rateData[0]["rate"];
        }

        Product product = new Product(item["id"].ToString(), item["name"], item["ratesum"], item["ratecount"], follow, rated, myRate);

        return product;
    }
}