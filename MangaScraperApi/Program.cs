using Hangfire;
using MangaScraperApi.Services;
using MangaScraperApi.Interfaces.ServiceInterfaces;
using MangaScraperApi.Models.Settings;
using Microsoft.EntityFrameworkCore;
using MangaScraper.Data.Data;
using MangaScraper.Data.Interfaces;
using MangaScraper.Data.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MangaScraper.Data.Models.Auth;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// Create confiuration
IConfigurationRoot configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Registrazione del DbContext
builder.Services.AddDbContext<AppDbContext>(o =>
    o.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

//Registrazione del servizio di Autenticazione -> IMPORTANTE, CONTROLLARE SE SERVE, EVITARE DI INIETTARE QUESTO COME DIPENDENZA
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            RequireExpirationTime = true,
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = configuration.GetSection("JwtSettings").GetValue<string>("Issuer"),
            ValidAudience = configuration.GetSection("JwtSettings").GetValue<string>("Audience"),
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetSection("JwtSettings").GetValue<string>("SecretKey") ?? throw new Exception())
            )
        };
    });

builder.Services.AddLogging(logging => logging.AddSimpleConsole(options =>
    {
        options.IncludeScopes = true;
        //options.SingleLine = true;
        options.TimestampFormat = "yyyy/MM/d HH:mm:ss:fff";
    }));

builder.Services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 1;
});

builder.Services.AddScoped<IMangaScraperService, MangaScraperService>();
builder.Services.AddSingleton<ISeleniumService, SeleniumService>();
builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddSingleton<HttpClient>();
builder.Services.AddSingleton<Settings>();
builder.Services.AddSingleton(configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHangfireDashboard("/hangfire");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();
    //.RequireAuthorization();

app.Run();
