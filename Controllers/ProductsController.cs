using System.Globalization;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using rateit.Helpers;
using rateit.Interfaces;
using rateit.Models;
using rateit.Products;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;

namespace rateit.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
    private const int ObjPerPage = 20;

    private readonly ISqlManager _sqlManager;
    private readonly IGetObject _getObject;
    private readonly ITokenManager _tokenVerification;

    private const string BaseUrl = "/products";

    public ProductsController(ISqlManager sqlManager, IGetObject getObject, ITokenManager tokenVerification)
    {
        _sqlManager = sqlManager;
        _getObject = getObject;
        _tokenVerification = tokenVerification;
    }

    [HttpPost($"{BaseUrl}/search")]
    public async Task<IActionResult> Search(SearchRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        List<Dictionary<string, dynamic>> data;

        try
        {

            if (request.Query != "")
            {
                data = await _sqlManager.Reader(
                    $"SELECT id FROM products.products WHERE lower(name) LIKE lower('{request.Query}%') OR lower(producer) LIKE lower('% {request.Query}%') OR lower(ean) LIKE lower('% {request.Query}%') ORDER BY sponsor DESC, ((ratesum)/(ratecount)) DESC, id LIMIT {(request.Page + 1) * ObjPerPage};");
            }
            else
            {
                data = await _sqlManager.Reader(
                    $"SELECT id FROM products.products ORDER BY sponsor DESC, ((ratesum)/(ratecount)) DESC, id LIMIT {(request.Page + 1) * ObjPerPage};");
            }

        }
        catch
        {
            return StatusCode(409, "walony err");
        }

        List<Product> result = new List<Product>();



        for (int i = (request.Page * 1) * ObjPerPage; i != (request.Page + 1) * ObjPerPage; i++)
        {
            if (i == data.Count - 1) break;
            result.Add(await _getObject.GetProduct(data[i]["id"], request.UserId));
        }


        return new ObjectResult(result);

    }

    [HttpPost($"{BaseUrl}/deleteNote")]
    public async Task<IActionResult> Unfollow(FollowRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        bool follow = await _sqlManager.IsValueExist(
            $"SELECT * FROM user_details.my_product_{request.UserId} WHERE productid = {request.ProductId};");
        if (!follow) return StatusCode(409, "ProductDoesNotHaveNote");

        await _sqlManager.Execute(
            $"DELETE FROM user_details.my_product_{request.UserId} WHERE productid = {request.ProductId};");

        return Ok();
    }

    [HttpPost($"{BaseUrl}/rate")]
    public async Task<IActionResult> Rate(RateRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        if (await _sqlManager.IsValueExist(
                $"SELECT * FROM user_details.rated_products_{request.UserId} WHERE productid = '{request.ProductId}';"))
        {
            int lastMyRate =
                (await _sqlManager.Reader(
                    $"SELECT * FROM user_details.rated_products_{request.UserId} WHERE productid = '{request.ProductId}';"))
                [0]["rate"];
            int beforeRate =
                (await _sqlManager.Reader($"SELECT ratesum FROM products.products WHERE id = '{request.ProductId}';"))
                [0]["ratesum"];

            int newRate = (beforeRate - lastMyRate) + request.Rate;


            await _sqlManager.Execute(
                $"UPDATE user_details.rated_products_{request.UserId} SET rate = {request.Rate} WHERE productid = {request.ProductId}");
            await _sqlManager.Execute(
                $"UPDATE products.products SET ratesum = {newRate} WHERE id = '{request.ProductId}'");
        }
        else
        {
            await _sqlManager.Execute(
                $"INSERT INTO user_details.rated_products_{request.UserId} VALUES ({request.ProductId}, {request.Rate})");
            await _sqlManager.Execute(
                $"UPDATE products.products SET ratesum = ratesum + {request.Rate} WHERE id = '{request.ProductId}'");

            if (await _sqlManager.IsValueExist(
                    $"SELECT * FROM products.products WHERE id = '{request.ProductId}' AND ratecount = -1;"))
            {
                await _sqlManager.Execute(
                    $"UPDATE products.products SET ratecount = ratecount + 2 WHERE id = '{request.ProductId}'");
            }
            else
            {
                await _sqlManager.Execute(
                    $"UPDATE products.products SET ratecount = ratecount + 1 WHERE id = '{request.ProductId}'");
            }
        }

        return Ok();

    }

    [HttpPost($"{BaseUrl}/unrate")]
    public async Task<IActionResult> Unrate(UnrateRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        if (await _sqlManager.IsValueExist(
                $"SELECT * FROM user_details.rated_products_{request.UserId} WHERE productid = {request.ProductId};"))
        {
            int lastMyRate =
                (await _sqlManager.Reader(
                    $"SELECT * FROM user_details.rated_products_{request.UserId} WHERE productid = '{request.ProductId}';"))
                [0]["rate"];

            await _sqlManager.Execute(
                $"DELETE FROM user_details.rated_products_{request.UserId} WHERE productid = {request.ProductId}");

            if (await _sqlManager.IsValueExist(
                    $"SELECT * FROM products.products WHERE id = '{request.ProductId}' AND ratecount = 1;"))
            {
                await _sqlManager.Execute(
                    $"UPDATE products.products SET ratecount = ratecount - 2 WHERE id = '{request.ProductId}'");
            }
            else
            {
                await _sqlManager.Execute(
                    $"UPDATE products.products SET ratecount = ratecount - 1 WHERE id = '{request.ProductId}'");
            }

            await _sqlManager.Execute(
                $"UPDATE products.products SET ratesum = ratesum - {lastMyRate} WHERE id = '{request.ProductId}'");

        }
        else
        {
            return StatusCode(409, "ProductIsNotRate");
        }

        return Ok();
    }

    [HttpPost($"{BaseUrl}/note")]
    public async Task<IActionResult> Note(NoteRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }


        await _sqlManager.Execute(
            $"INSERT INTO user_details.my_product_{request.UserId} VALUES({request.ProductId}, '{request.Note}')");
        return Ok();
    }

    [HttpPost($"{BaseUrl}/getRatedProduct")]
    public async Task<IActionResult> GetRatedProduct(GetRatedProductsRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        var data = await _sqlManager.Reader($"SELECT * FROM user_details.rated_products_{request.UserId};");


        List<Product> products = new List<Product>();
        foreach (var item in data)
        {
            products.Add(await _getObject.GetProduct(item["productid"].ToString
                (), request.UserId));
        }

        return new ObjectResult(products);
    }

    [HttpPost($"{BaseUrl}/getNotedProduct")]
    public async Task<IActionResult> GetFollowedProduct(GetRatedProductsRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        var data = await _sqlManager.Reader($"SELECT * FROM user_details.my_product_{request.UserId};");


        List<Product> products = new List<Product>();
        foreach (var item in data)
        {
            products.Add(await _getObject.GetProduct(item["productid"].ToString(), request.UserId));
        }

        return new ObjectResult(products);
    }

    [HttpPost($"{BaseUrl}/checkProduct")]
    public async Task<IActionResult> CheckProduct(CheckProductRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        if (await _sqlManager.IsValueExist($"SELECT * FROM products.products WHERE ean = '{request.Ean}';"))
        {
            string id =
                (await _sqlManager.Reader($"SELECT * FROM products.products WHERE ean = '{request.Ean}';"))[0]["id"]
                .ToString();
            return new ObjectResult(await _getObject.GetProduct(id, request.UserId));
        }

        try
        {

            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get,
                new Uri(
                    $"https://api.barcodelookup.com/v3/products?barcode={request.Ean}&formatted=y&key=i4liud7cw31ebt07j0i5cuphgonvbx")));

            dynamic content = JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync()) ?? new object();

            if (response.StatusCode == HttpStatusCode.NotFound) return StatusCode(409, "ProductNotFound");

            string name = content["products"][0]["title"];
            string image = content["products"][0]["images"][0];
            string producer = content["products"][0]["category"];
            string category = content["products"][0]["brand"];
            string ean = request.Ean;
            


            return new ObjectResult(new Product(0.ToString(), name, 0, 0, false, false, 0, image, category, ean, producer));
        }
        catch (Exception e)
        {
            return StatusCode(409, "ApiBardCodeError");
            throw;
        }
    }

    [HttpPost($"{BaseUrl}/addProduct")]
    public async Task<IActionResult> AddProduct(AddProductRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        int id;
        while (true)
        {
            Random rand = new Random();
            id = rand.Next(100000000, 999999990);
            if (!await _sqlManager.IsValueExist($"SELECT * FROM products.products WHERE id = '{id}';") &&
                !await _sqlManager.IsValueExist($"SELECT * FROM products.orders WHERE id = '{id}';"))
                break;
        }

        await _sqlManager.Execute(
            $"INSERT INTO products.orders VALUES ('{request.Ean}', {request.UserId}, '{request.Name}', '{id}', '{request.Image}', '{request.Producer}', {request.CategoryId});");

        return Ok();
    }

    [HttpPost($"{BaseUrl}/viewProduct")]
    public async Task<IActionResult> ViewProduct(ViewProductRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        await _sqlManager.Execute(
            $"INSERT INTO user_details.last_view_products_{request.UserId} VALUES({request.ProductId}, NOW());");
        return Ok();
    }

    [HttpPost($"{BaseUrl}/getViewedProduct")]
    public async Task<IActionResult> GetViewedProduct(GetViewedProductRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        var data = await _sqlManager.Reader(
            $"SELECT * FROM user_details.last_view_products_{request.UserId} ORDER BY date");

        List<Product> products = new List<Product>();

        for (int i = (request.Page * 1) * ObjPerPage; i != (request.Page + 1) * ObjPerPage; i++)
        {
            try
            {
                products.Add(await _getObject.GetProduct(data[i]["productid"].ToString(), request.UserId));
            }
            catch (Exception e)
            {
                //ignore
            }
        }

        return new ObjectResult(products);
    }

    [HttpPost($"{BaseUrl}/getCategories")]
    public async Task<IActionResult> GetCategories(AuthRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        return new ObjectResult(await _sqlManager.Reader($"SELECT * FROM products.categories;"));
    }

    [HttpPost($"{BaseUrl}/getSubcategories")]
    public async Task<IActionResult> GetSubcategories(GetSubcategoriesRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        return new ObjectResult(await _sqlManager.Reader(
            $"SELECT id, name FROM products.subcategories WHERE overcategory = {request.CategoryId};"));
    }

    [HttpPost($"{BaseUrl}/getCategoryRanking")]
    public async Task<IActionResult> GetCategoryRanking(GetCategoryRankingRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        var data = await _sqlManager.Reader(
            $"SELECT * FROM products.products WHERE category = {request.CategoryId} ORDER BY ((ratesum)/(ratecount)) DESC LIMIT {ObjPerPage * (request.Page + 1)};");
        List<Product> result = new List<Product>();

        for (int i = request.Page * ObjPerPage; i != (request.Page + 1) * ObjPerPage; i++)
        {
            try
            {
                result.Add(await _getObject.GetProduct(data[i]["id"].ToString(), request.UserId));
            }
            catch
            {
                // ignored
            }
        }

        return new ObjectResult(result);
    }
}