using System.Net.Mime;
using Hack2022;
using Microsoft.AspNetCore.Mvc;
using rossgram;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<UserDataDBOptions>(builder.Configuration.GetSection("Connection"));
builder.Services.AddCors();
builder.Services.AddSingleton<UserDataService>();
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

app.Run();
