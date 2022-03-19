using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using rossgram;

public class AccountController : Controller
{
    [HttpPost("/token")]
    public IActionResult Token(string username, string password,UserDataService service)
    {
        var identity = GetIdentity(username, password, service);
        if (identity == null)
        {
            return BadRequest(new {errorText = "Invalid username or password."});
        }

        var now = DateTime.UtcNow;
        // создаем JWT-токен
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));
        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        var response = new
        {
            access_token = encodedJwt,
            username = identity.Name
        };

        return Json(response);
    }
    [HttpPost("/register")]
    public IActionResult Register(string username, string password,UserDataService service)
    {
        var identity = GetIdentity(username, password,service);
        if (identity == null)
        {
            service.AddUser(new UserModel(username,password,"user"));
            //people.Add(new Person{Login = username,Password = password,Role = "user"});
        }
        return Token(username, password, service);
    }

    private ClaimsIdentity GetIdentity(string username, string password,UserDataService service)
    {
        var User = service.Getuser(login: username);
        if (User.Password != password) User = null;
        if (User != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, User.Login),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, User.Role)
            };
            ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        // если пользователя не найдено
        return null;
    }
}