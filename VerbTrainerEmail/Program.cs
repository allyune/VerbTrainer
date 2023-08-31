using Microsoft.EntityFrameworkCore;
using VerbTrainer.Infrastructure.Messaging.Consumer;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Infrastructure;
using VerbTrainerEmail.Infrastructure.Data;
using VerbTrainerEmail.Infrastructure.Messaging.Configuration;
using VerbTrainerEmail.Infrastructure.Messaging.Consumer;
using VerbTrainerEmail.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<EmailDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("VerbTrainerAuthConnectionString")));

builder.Services.AddScoped<IAsyncEmailRepository, AsyncEmailRepository>();
builder.Services.AddScoped<IAsyncUserRepository, AsyncUserRepository>();
builder.Services.AddTransient<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
builder.Services.AddHostedService<ConsumerHostedService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

