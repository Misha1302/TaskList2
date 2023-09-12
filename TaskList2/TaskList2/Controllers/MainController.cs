namespace TaskList2.Controllers;

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

[ApiController]
[Route("[controller]/[action]")]
public class MainController : ControllerBase
{
    private readonly IPersonRepository _personRepository;

    public MainController(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    [HttpPost]
    public IResult Login(Person p)
    {
        // находим пользователя 
        var person = _personRepository.Get(p.Id);
        if (person?.Password != p.Password)
            return Results.Unauthorized();

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
            user_id = person.Id
        };

        return Results.Json(response);
    }

    [HttpPost]
    public IResult Authorize(Person p)
    {
        // находим пользователя 
        var person = _personRepository.Get(p.Id);
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
        _personRepository.Create(p);

        // формируем ответ
        var response = new
        {
            access_token = encodedJwt,
            user_id = p.Name
        };

        return Results.Json(response);
    }

    [HttpPost]
    public IResult Data(string token)
    {
        if (new JwtSecurityTokenHandler().ReadJwtToken(token) == default)
            return Results.Unauthorized();

        var data = new { data = "Hi!" };
        return Results.Json(data);
    }
}