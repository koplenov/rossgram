using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using rossgram;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<UserDataDBOptions>(builder.Configuration.GetSection("Connection"));
builder.Services.AddCors();
builder.Services.AddSingleton<UserDataService>();

//
var services = builder.Services;
services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // укзывает, будет ли валидироваться издатель при валидации токена
            ValidateIssuer = true,
            // строка, представляющая издателя
            ValidIssuer = AuthOptions.ISSUER,

            // будет ли валидироваться потребитель токена
            ValidateAudience = true,
            // установка потребителя токена
            ValidAudience = AuthOptions.AUDIENCE,
            // будет ли валидироваться время существования
            ValidateLifetime = true,

            // установка ключа безопасности
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            // валидация ключа безопасности
            ValidateIssuerSigningKey = true,
        };
    });
services.AddControllersWithViews();

//
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/photos/{userName}", (string userName,UserDataService service) =>
{
    return Results.Ok(service.Getuser(userName).Images);
});
app.MapPost("/photo/{userName}", ([FromBody]string photo,string userName, UserDataService service) =>
{
    var user = service.Getuser(userName);
    user.Images.Add(photo);
    service.UpdateUser(userName,user);
    return Results.Ok("posted");
});


app.ApplyUserKeyValidation();
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

//
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapDefaultControllerRoute(); });
//

app.Run();
