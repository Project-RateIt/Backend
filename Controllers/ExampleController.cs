using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rateit.Helpers;
using rateit.Interfaces;

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

    private const string BaseUrl = "/example";
    
    public ExampleController(ISqlManager sqlManager, ILogger<ExampleController> logger,
        IGetObject getObject, 
        IEmailManager emailManager, ITokenManager tokenVerification)
    {
        _sqlManager = sqlManager;
        _logger = logger;
        _getObject = getObject;
        _emailManager = emailManager;
        _tokenVerification = tokenVerification;
    }
    
    [HttpPost($"{BaseUrl}/ExampleEndpoint")]
    public async Task<IActionResult> ExampleEndpoint()
    {
        if (!await _tokenVerification.UserVerification("token z logowania", UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }
        
        return Ok();
    }
    
    [HttpGet($"{BaseUrl}/test")]
    public async Task<IActionResult> Test()
    {
        await _sqlManager.Execute($"INSERT INTO xd VALUES (12);");
        
        return new ObjectResult(await _sqlManager.Reader($"SELECT * FROM xd;"));
    }
}