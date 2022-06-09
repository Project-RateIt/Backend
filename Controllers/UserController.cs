using BCrypt.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using rateit.Helpers;
using rateit.Models;
using rateit.User;

namespace rateit.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly ISqlManager _sqlManager;
    private readonly IGetObject _getObject;
    private readonly ILogger<UserController> _logger;
    private readonly IEmailManager _emailManager;
    private readonly ITokenManager _tokenVerification;

    private const string BaseUrl = "/user";
    
    public UserController(ISqlManager sqlManager, ILogger<UserController> logger,
        IGetObject getObject, 
        IEmailManager emailManager, ITokenManager tokenVerification)
    {
        _sqlManager = sqlManager;
        _logger = logger;
        _getObject = getObject;
        _emailManager = emailManager;
        _tokenVerification = tokenVerification;
    }
    
    [HttpPost($"{BaseUrl}/register")]
    public async Task<IActionResult> Register(UserRegisterRequestModel request)
    {
        int id;         
        while (true)
        {
            Random rand = new Random();
            id = rand.Next(10000000, 99999999);          
            if (!await _sqlManager.IsValueExist($"SELECT * FROM users.users WHERE id = {id};"))
                break;
        }

        if (await _sqlManager.IsValueExist($"SELECT * FROM users.users WHERE email = '{request.Email}';"))
            throw new Exception("EmailIsExist");
        
        await _sqlManager.Execute($"INSERT INTO users.users VALUES({id}, '{request.Name}', '{request.Surname}', '{request.Email}',false, '{BCrypt.Net.BCrypt.HashPassword(request.Password, SaltRevision.Revision2Y)}');");
        await _sqlManager.Execute($"CREATE TABLE user_details.last_view_products_{id} (productid int);");
        await _sqlManager.Execute($"CREATE TABLE user_details.rated_products_{id} (productid int, rate int);");
        await _sqlManager.Execute($"CREATE TABLE user_details.my_product_{id} (productid int, note varchar);");
        
        Models.User user;
        
        try
        {
            user = await _getObject.GetUser(id);
        }
        catch (UserIsNotExist)
        {
            return StatusCode(409, "UnexpectedException");
        }
        //await _emailManager.SendEmail(user.Email);
        //TODO send mail email and verification

        return new ObjectResult(user);
    }
    [HttpPost($"{BaseUrl}/login")]
    public async Task<IActionResult> Login(LoginUserRequestModel request)
    {
        Models.User user;

        if (await _sqlManager.IsValueExist($"SELECT id FROM users.users WHERE email = '{request.Email}';"))
        {
            var data = (await _sqlManager.Reader($"SELECT id, password, isactivated FROM users.users WHERE email = '{request.Email}';"))[0];

            if (!data["isactivated"])
            {
                return StatusCode(409, "UserIsNotActive");
            }
            if (BCrypt.Net.BCrypt.Verify(request.Password, data["password"]))
            {
                user = await _getObject.GetUser(data["id"]);
            }
            else
            {
                return StatusCode(409, "BadPassword");
            }
        }
        else
        {
            return StatusCode(409, "UserIsNotExist");
        }

        string newToken = await _tokenVerification.GenerateToken(user.Id);

        return new ObjectResult(new LoginUserResponseModel(user, newToken));
    }
    [HttpPost($"{BaseUrl}/isUserExist")]
    public async Task<IActionResult> IsUserExist(IsUserExistRequestModel request)
    {
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }
        
        Models.User user;
        try
        {
            user = await _getObject.GetUser(request.Id);
        }
        catch (UserIsNotExist)
        {
            return StatusCode(409, "UserIsNotExist");
        }

        return new ObjectResult(user);
    }
    [HttpPost($"{BaseUrl}/settings")]
    public async Task<IActionResult> SetSettings(SettingsRequestModel request)
    {
        
        if (!await _tokenVerification.UserVerification(request.Token, UserType.User))
        {
            return StatusCode(409, "BadAccessToken");
        }
        if (!await _sqlManager.IsValueExist($"SELECT id FROM users.users WHERE id = {request.Id}"))
            return StatusCode(409, "UserIsNotExist");
        

        
        switch (request.Mode)
        {
            case SettingsMode.Name:
            {
                await _sqlManager.Execute($"UPDATE users.users SET name = '{request.Value}' WHERE id = {request.Id};");
                break;
            }
            case SettingsMode.Surname:
            {
                await _sqlManager.Execute($"UPDATE users.users SET surname = '{request.Value}' WHERE id = {request.Id};");
                break;
            }
            case SettingsMode.Email:
            {
                await _sqlManager.Execute($"UPDATE users.users SET email = '{request.Value}' WHERE id = {request.Id};");
                break;
            }
            case SettingsMode.Avatar:
            {
                await System.IO.File.WriteAllTextAsync($@"Avatars\{request.Id}", request.Value.ToString());
                await _sqlManager.Execute($"UPDATE users.users SET hasAvatar = true WHERE id = {request.Id};");

                break;
            }
            default:
                throw new ArgumentOutOfRangeException();
        }

        return Ok();
    }
}