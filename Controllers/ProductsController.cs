using Microsoft.AspNetCore.Mvc;
using rateit.Helpers;
using rateit.Interfaces;
using rateit.Models;
using rateit.Products;

namespace rateit.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductsController : ControllerBase
{
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
        var data = await _sqlManager.Reader($"SELECT id FROM products.products WHERE name LIKE '%{request.Query}%' OR keywords LIKE '%{request.Query}%' ORDER BY sponsor DESC, (ratesum/ratecount) DESC LIMIT {(request.Page + 1) * 10};");

        List<Product> result = new List<Product>();
        
        foreach (var item in data)
        {
            result.Add(await _getObject.GetProduct(item["id"], request.UserId));         
        }

        return new ObjectResult(result);
    }
    
    [HttpPost($"{BaseUrl}/follow")]
    public async Task<IActionResult> Follow(FollowRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }
        
        bool follow = await _sqlManager.IsValueExist($"SELECT * FROM user_details.my_product_{request.UserId} WHERE productid = {request.ProductId};");
        if (follow) return StatusCode(409, "IsFollow");

        await _sqlManager.Execute($"INSERT INTO user_details.my_product_{request.UserId} VALUES({request.ProductId}, '')");

        return Ok();
    }    
    [HttpPost($"{BaseUrl}/unfollow")]
    public async Task<IActionResult> Unfollow(FollowRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }
        
        bool follow = await _sqlManager.IsValueExist($"SELECT * FROM user_details.my_product_{request.UserId} WHERE productid = {request.ProductId};");
        if (!follow) return StatusCode(409, "IsNotFollow");

        await _sqlManager.Execute($"DELETE FROM user_details.my_product_{request.UserId} WHERE productid = {request.ProductId};");

        return Ok();
    }
    [HttpPost($"{BaseUrl}/rate")]
    public async Task<IActionResult> Rate(RateRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }

        if (await _sqlManager.IsValueExist($"SELECT * FROM user_details.rated_products_{request.UserId} WHERE productid = {request.ProductId};"))
        {
            await _sqlManager.Execute(
                $"UPDATE user_details.rated_products_{request.UserId} SET rate = {request.Rate} WHERE productid = {request.ProductId}");
        }
        else
        {
            await _sqlManager.Execute(
                $"INSERT INTO user_details.rated_products_{request.UserId} VALUES ({request.ProductId}, {request.Rate})");
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
        if (await _sqlManager.IsValueExist($"SELECT * FROM user_details.rated_products_{request.UserId} WHERE productid = {request.ProductId};"))
        {
            await _sqlManager.Execute(
                $"DELETE FROM user_details.rated_products_{request.UserId} WHERE productid = {request.ProductId}");
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
        
        return Ok();
    }    
}