namespace TaskList2.Controllers;

using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class AuthOptions
{
    public const string Issuer = "MyAuthServer"; // издатель токена
    public const string Audience = "MyAuthClient"; // потребитель токена
    private const string Key = "mysupersecret_secretkey!12345678"; // ключ для шифрации

    public static SymmetricSecurityKey GetSymmetricSecurityKey()
    {
        if (Key.Length < 32)
            throw new InvalidOperationException("Len of key must be greater than 32");

        return new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));
    }
}