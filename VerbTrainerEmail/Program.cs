using Microsoft.EntityFrameworkCore;
using VerbTrainerEmail.Domain.Interfaces;
using VerbTrainerEmail.Infrastructure;
using VerbTrainerEmail.Infrastructure.Data;
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

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

