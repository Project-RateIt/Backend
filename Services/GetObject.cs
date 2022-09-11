using rateit.Interfaces;
using rateit.Models;

namespace rateit.Services;

public class GetObject : IGetObject
{
    private readonly ISqlManager _sqlManager;

    public GetObject(ISqlManager sqlManager)
    {
        _sqlManager = sqlManager;
    }

    public async Task<Models.User> GetUser(int id)
    {
        var date = await _sqlManager.Reader($"SELECT * FROM users.users WHERE id = {id};");

        if (date.Count == 0) throw new Exception("ErrGetUser");
        
        Models.User user = new Models.User(date[0]["id"], date[0]["name"], date[0]["surname"], date[0]["email"], await _sqlManager.IsValueExist($"SELECT * FROM users.admin WHERE id = {id};"), date[0]["haveavatar"]);
        
        return user;
    }

    public async Task<Product> GetProduct(string id, int userId)
    {
        var data = await _sqlManager.Reader($"SELECT * FROM products.products WHERE id = '{id}';");

        if (data.Count == 0) return new Product();
        
        var item = data[0];

        bool follow = await _sqlManager.IsValueExist($"SELECT * FROM user_details.my_product_{userId} WHERE productid = '{id}';");

        List<Dictionary<string, dynamic>> rateData = await _sqlManager.Reader($"SELECT * FROM user_details.rated_products_{userId} WHERE productid = '{id}';");

        bool rated = rateData.Count > 0;
        int myRate = 0;
        if (rated)
        {
            myRate = rateData[0]["rate"];
        }
        
        var subcategoryData = (await _sqlManager.Reader($"SELECT * FROM products.subcategories WHERE id = {item["category"]}"))[0];
        var categoryData = (await _sqlManager.Reader($"SELECT * FROM products.categories WHERE id = {subcategoryData["overcategory"]}"))[0];

        
        Subcategory category = new Subcategory(categoryData["id"], categoryData["name"]);
        Subcategory subcategory = new Subcategory(subcategoryData["id"], subcategoryData["name"]);

        int placeInRanging = 0;
        if (category.Id != 10)
        {
            var rankingInSubcategories = await _sqlManager.Reader($"SELECT * FROM products.products WHERE category = {item["category"]} ORDER BY (ratesum/ratecount) DESC");
            foreach (var p in rankingInSubcategories)
            {
                placeInRanging += 1;
                if (p["id"] == item["id"])
                {
                    break;
                }
            }
        }

        Product product = new Product(item["id"].ToString(), item["name"], (int)item["ratesum"], (int)item["ratecount"], follow, rated, myRate, item["img"], item["ean"], item["producer"], placeInRanging, subcategory, category);

        return product;
    }
}