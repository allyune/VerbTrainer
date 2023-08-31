using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using VerbTrainerAuth.Data;
using VerbTrainerAuth.Services;
using VerbTrainerAuth.Infrastructure.Messaging.Configuration;
using VerbTrainerAuth.Infrastructure.Messaging.Producer;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");

string validAudience = builder.Configuration["JwtSettings:Audience"];
string validIssuer = builder.Configuration["JwtSettings:Issuer"];
string secretKey = builder.Configuration["JwtSettings:Key"];
var rabbitMqSection = builder.Configuration.GetSection("MessagingSettings");

// Add services to the container.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = true,
        ValidAudience = validAudience,
        ValidateIssuer = true,
        ValidIssuer = validIssuer,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddDbContext<VerbTrainerAuthDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("VerbTrainerAuthConnectionString")));
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
builder.Services.AddScoped<IMessagingProducer, MessagingProducer>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
