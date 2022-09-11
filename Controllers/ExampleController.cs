using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rateit.Helpers;
using System.IO;
using rateit.Interfaces;
using rateit.Models;

namespace rateit.Controllers;

[ApiController]
[Route("[controller]")]
public class ExampleController : ControllerBase
{
    private readonly ISqlManager _sqlManager;
    private readonly IGetObject _getObject;
    private readonly ILogger<ExampleController> _logger;
    private readonly IEmailManager _emailManager;
    private readonly ITokenManager _tokenVerification;
    private readonly IConfiguration _configuration;

    private const string BaseUrl = "/example";
    
    public ExampleController(ISqlManager sqlManager, ILogger<ExampleController> logger,
        IGetObject getObject, 
        IEmailManager emailManager, ITokenManager tokenVerification, IConfiguration configuration)
    {
        _sqlManager = sqlManager;
        _logger = logger;
        _getObject = getObject;
        _emailManager = emailManager;
        _tokenVerification = tokenVerification;
        _configuration = configuration;
    }
    
    [HttpPost($"{BaseUrl}/ExampleEndpoint")]
    public async Task<IActionResult> ExampleEndpoint()
    {
        return Ok();
    }

    [HttpPost($"/")]
    public async Task<IActionResult> Test()
    {
        int a = 0;
        
        return new ObjectResult((1/a));
    } 

    
    [HttpGet($"{BaseUrl}/category")]
    public async Task<IActionResult> Category(string pass)
    {

        if (pass != "superjestesniezmienajsie") return new AcceptedResult();


        await _sqlManager.Execute($"DELETE FROM products.categories WHERE id > 11");
        await _sqlManager.Execute($"DELETE FROM products.subcategories WHERE id > 11");
        
        
        StreamReader sr = new StreamReader("category");
        string? line;

        Random rand = new Random();
        int category = 0;

        while (!string.IsNullOrEmpty(line = (await sr.ReadLineAsync())?.Trim(':')))
        {
            if (Char.IsUpper(line.ToCharArray()[0]))
            {
                category = rand.Next(100000, 999999);
                await _sqlManager.Execute($"INSERT INTO products.categories VALUES({category}, '{line.ToLower()}') ");
            }
            else
            {
                await _sqlManager.Execute($"INSERT INTO products.subcategories VALUES({rand.Next(100000, 999999)}, '{line}', {category}, '') ");
            }
        }

        var p = await _sqlManager.Reader($"SELECT name, id FROM products.products ORDER BY id;");
        int progress = 0;
        int addedProduct = 0;

        var cat = await _sqlManager.Reader("SELECT * FROM products.subcategories");

        
        await _sqlManager.Execute($"UPDATE products.products SET category = {1}");

        
        foreach (var product in p)
        {

            foreach (var categoryToCheck in cat)
            {
                if ((product["name"].ToLower()).Contains(categoryToCheck["name"].ToLower()))
                {
                    await _sqlManager.Execute($"UPDATE products.products SET category = {categoryToCheck["id"]} WHERE id = '{product["id"]}'");
                    addedProduct += 1;
                }
            }

            progress += 1;
            Console.WriteLine(progress + " / 14260 " + addedProduct);
        }

        foreach (var sc in cat)
        {
            if (!(await _sqlManager.IsValueExist($"SELECT * FROM products.products WHERE category = {sc["id"]}")))
            {
                await _sqlManager.Execute($"DELETE FROM products.subcategories WHERE id = {sc["id"]}");
            }
        }
        
        return new OkResult();
    }
}