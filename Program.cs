using rossgram;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<UserDataDBOptions>(builder.Configuration.GetSection("Connection"));
builder.Services.AddCors();
//builder.Services.AddSingleton<>();
var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.UseCors(c => c.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());

app.Run();
