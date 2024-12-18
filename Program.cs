using System.Text;
using API;
using API.Data;
using API.Extensions;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplicationservices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().
    WithOrigins("http://localhost:4200","https://localhost:4200"));
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
