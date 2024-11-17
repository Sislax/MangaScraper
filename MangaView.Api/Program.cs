using MangaScraper.Data.Data;
using MangaScraper.Data.Interfaces;
using MangaScraper.Data.Repositories;
using MangaView.Api.Interfaces;
using MangaView.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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

//Registrazione dell'Identity System -> IMPORTANTE, CONTROLLARE SE SERVE, EVITARE DI INIETTARE QUESTO COME DIPENDENZA
//builder.Services.AddIdentity<User, Role>(options =>
//    {
//        options.SignIn.RequireConfirmedAccount = true;
//    
//        //Password Settings
//        options.Password.RequireDigit = true;
//        options.Password.RequireLowercase = true;
//        options.Password.RequireUppercase = true;
//        options.Password.RequireNonAlphanumeric = true;
//        options.Password.RequiredLength = 8;
//        options.Password.RequiredUniqueChars = 4;
//    
//        //Lockout Settings
//        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
//        options.Lockout.MaxFailedAccessAttempts = 7;
//        options.Lockout.AllowedForNewUsers = true;
//    
//        //User Settings
//        options.User.AllowedUserNameCharacters =
//            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789_-.:,;^?=()&%$£!|";
//    })
//    .AddEntityFrameworkStores<UserIdentityDbContext>()
//    .AddDefaultTokenProviders();

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
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
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
        options.TimestampFormat = "yyyy/MM/d HH:mm:ss:fff";
    }));

builder.Services.AddScoped<IAppRepository, AppRepository>();
builder.Services.AddScoped<IMangaService, MangaService>();
builder.Services.AddSingleton<IImageSharpService, ImageSharpService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
