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
}