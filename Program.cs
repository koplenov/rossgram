using System.Net.Mime;
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
app.MapPost("/photo", ([FromBody]UserModel model, UserDataService service) =>
{
    service.AddUser(model);
    return Results.Ok("posted");
});

app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();


//byte[] imageArray = System.IO.File.ReadAllBytes(@"1.png");
//string base64ImageRepresentation = Convert.ToBase64String(imageArray);


