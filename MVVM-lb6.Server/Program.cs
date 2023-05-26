using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Server;
using System.Text;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MVVM_lb6.Server.Common.Constans;
using MVVM_lb6.Server.Models;
using Server.Services;
using Server.Services.Abstract;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind("Settings", settings);
builder.Services.AddSingleton(settings);

var localDatabase = builder.Configuration.GetConnectionString(ConnectionKeys.Local);
builder.Services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite(localDatabase));

builder.Services.AddControllers().AddNewtonsoftJson(i =>
{
    
});

builder.Services.AddScoped<IHashProvider, HashProvider>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters()
    {
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(settings.BearerKey)),
        ValidateIssuerSigningKey = true,
        ValidateAudience = false,
        ValidateIssuer = false,
    };
});

var app = builder.Build(); // Создает обьект WebApplication

if (app.Environment.IsDevelopment()) 
{
    
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();

//For test
public partial class Program { }
