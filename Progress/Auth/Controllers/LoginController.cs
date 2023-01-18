using System.Net.Mime;
using System.Text;
using Auth.Providers.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using View.Models.In;
using View.Models.Out;

namespace Auth.Controllers;

[Produces(MediaTypeNames.Application.Json)]
[Consumes(MediaTypeNames.Application.Json)]
[Route("login"), ApiController]
public class LoginController : ControllerBase
{
    private readonly IUserProvider _userProvider;
    public LoginController(IUserProvider userProvider)
    {
        _userProvider = userProvider;
    }
    
    [HttpPost, AllowAnonymous]
    [ProducesResponseType(typeof(OutUserView), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> SignIn([FromBody] InAuthView loginData)
    {
        var userResult = await _userProvider.AuthenticateAsync(loginData);
        /*// находим пользователя 
        var user = people.FirstOrDefault(p => p.Email == loginData.Email && p.Password == loginData.Password);
        // если пользователь не найден, отправляем статусный код 401
        if(user is null) return new UnauthorizedResult();
     
        var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.Email) };
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            claims: claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
 */
        return new OkObjectResult(userResult);
    }
}

public record class Person(string Email, string Password);
public class AuthOptions
{
    public const string ISSUER = "MyAuthServer"; // издатель токена
    public const string AUDIENCE = "MyAuthClient"; // потребитель токена
    const string KEY = "mysupersecret_secretkey!123";   // ключ для шифрации
    public static SymmetricSecurityKey GetSymmetricSecurityKey() => 
        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
}