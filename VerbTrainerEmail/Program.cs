using Microsoft.EntityFrameworkCore;
using VerbTrainer.Infrastructure.Messaging.Consumer;
using VerbTrainerEmail.Application.SendPasswordResetEmail;
using VerbTrainerEmail.Application.SendPasswordResetEmail.Handler;
using VerbTrainerEmail.Application.Services.SendEmail;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Infrastructure;
using VerbTrainerEmail.Infrastructure.Data;
using VerbTrainerEmail.Infrastructure.Messaging.Configuration;
using VerbTrainerEmail.Infrastructure.Messaging.Consumer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmailDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("VerbTrainerAuthConnectionString")));

builder.Services.AddScoped<IAsyncEmailRepository, AsyncEmailRepository>();
builder.Services.AddScoped<ISendPasswordResetEmail, SendPasswordResetEmail>();
builder.Services.AddScoped<IPasswordResetEmailHandler, PasswordResetEmailHandler>();
builder.Services.AddTransient<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
builder.Services.AddHostedService<ConsumerHostedService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

