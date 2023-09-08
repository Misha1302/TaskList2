namespace TaskList2.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[controller]/[action]")]
public class MainController : ControllerBase
{
    private static readonly List<Person> _people = new()
    {
        new Person("tom@gmail.com", "12345"),
        new Person("bob@gmail.com", "55555")
    };

    [HttpPost]
    public IResult Login(Person p)
    {
        // находим пользователя 
        var person = _people.FirstOrDefault(x => x.Email == p.Email && x.Password == p.Password);
        // если пользователь не найден, отправляем статусный код 401
        if (person is null) return Results.Unauthorized();

        var claims = new List<Claim> { new(ClaimTypes.Name, person.Email) };
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        // формируем ответ
        var response = new
        {
            access_token = encodedJwt,
            username = person.Email
        };

        return Results.Json(response);
    }

    [HttpPost]
    public IResult Authorize(Person p)
    {
        // находим пользователя 
        var person = _people.FirstOrDefault(x => p.Email == x.Email);
        // если пользователь не найден, отправляем статусный код 401
        if (person is not null) return Results.Problem();

        var claims = new List<Claim> { new(ClaimTypes.Name, p.Email) };
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            AuthOptions.Issuer,
            AuthOptions.Audience,
            claims,
            expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        _people.Add(p);

        // формируем ответ
        var response = new
        {
            access_token = encodedJwt,
            username = p.Email
        };

        return Results.Json(response);
    }

    [HttpPost]
    [Authorize]
    public IResult Data()
    {
        var data = new { data = "Hi!" };
        return Results.Json(data);
    }
}