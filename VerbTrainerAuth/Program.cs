using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using VerbTrainerUser.Infrastructure.Data;
using VerbTrainerUser.Infrastructure.Messaging.Configuration;
using VerbTrainerUser.Infrastructure.Messaging.Producer;
using VerbTrainerUser.Domain.Interfaces;
using VerbTrainerUser.Infrastructure.Data.Repositories;
using VerbTrainerUser.Application.Services.Mapping;
using VerbTrainerUser.Application.Services.JWT;
using VerbTrainerUser.Application.Services.User;
using VerbTrainerUser.Application.UserLogin;
using VerbTrainerUser.Application.RefreshUserAccess;
using VerbTrainerUser.Application.UserRegister;
using VerbTrainerUser.Application.ResetPassword;
using Microsoft.IdentityModel.Logging;
using VerbTrainerEmail.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

IdentityModelEventSource.ShowPII = true;

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
builder.Services.AddScoped<IAsyncUserRepository, AsyncUserRepository>();
builder.Services.AddScoped<IAsyncRecoveryTokenRepository, AsyncRecoveryTokenRepository>();
builder.Services.AddScoped<IAsyncAccessTokenRepository, AsyncAccessTokenRepository>();
builder.Services.AddScoped<IAsyncRefreshTokenRepository, AsyncRefreshTokenRepository>();
builder.Services.AddScoped<IJWTService, JWTService>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IRecoveryTokenMapper, RecoveryTokenMapper>();
builder.Services.AddScoped<IUserMapper, UserMapper>();
builder.Services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
builder.Services.AddScoped<IMessagingProducer, MessagingProducer>();
builder.Services.AddScoped<IUserLoginHandler, UserLoginHandler>();
builder.Services.AddScoped<IResetPasswordHandler, ResetPasswordHandler>();
builder.Services.AddScoped<IRegisterUserHandler, RegisterUserHandler>();
builder.Services.AddScoped<IRefreshUserAccessHandler, RefreshUserAccessHandler>();

builder.Services.AddScoped<IUserService, UserService>();

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
