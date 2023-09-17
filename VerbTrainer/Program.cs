using Microsoft.EntityFrameworkCore;
using VerbTrainer.Infrastructure.DbInitializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using VerbTrainer.Infrastructure.Messaging.Configuration;
using VerbTrainer.Infrastructure.Messaging.Producer;
using VerbTrainer.Infrastructure.Data;
using VerbTrainer.Domain.Interfaces;
using VerbTrainer.Infrastructure.Repositories;
using VerbTrainer.Application.CreateDeck;
using VerbTrainer.Application.LoadDeck;
using VerbTrainer.Application.DeleteDeck;

var builder = WebApplication.CreateBuilder(args);
//builder.Configuration.AddJsonFile("appsettings.json");

string validAudience = builder.Configuration["JwtSettings:Audience"];
string validIssuer = builder.Configuration["JwtSettings:Issuer"];
string secretKey = builder.Configuration["JwtSettings:Key"];
var rabbitMqSection = builder.Configuration.GetSection("MessagingSettings");

// Add services to the container.
builder.Services.AddControllersWithViews();
//builder.Services.AddTransient<DbInitializer>();
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
builder.Services.AddDbContext<VerbTrainerDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("VerbTrainerConnectionString")));
builder.Services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
builder.Services.AddScoped<IMessagingProducer, MessagingProducer>();
builder.Services.AddScoped<IDeckRepository, DeckRepository>();
builder.Services.AddScoped<IDeckVerbRepository, DeckVerbRepository>();
builder.Services.AddScoped<ICreateDeckHandler, CreateDeckHandler>();
builder.Services.AddScoped<ILoadDeckHandler, LoadDeckHandler>();
builder.Services.AddScoped<IDeleteDeckHandler, DeleteDeckHandler>();
builder.Services.AddScoped<IAddVerbToDeckHandler, AddVerbToDeckHandler>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//	var services = scope.ServiceProvider;

//	var initialiser = services.GetRequiredService<DbInitializer>();

//	initialiser.Run();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
	name: "default",
	pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "GetDeckById",
    pattern: "api/deck/{deckid}",
    defaults: new { controller = "Deck", action = "GetDeck" }
);

app.MapControllerRoute(
    name: "AddDeck",
    pattern: "api/deck",
    defaults: new { controller = "Deck", action = "AddDeck" }
);

app.MapControllerRoute(
    name: "DeleteDeck",
    pattern: "api/deck",
    defaults: new { controller = "Deck", action = "DeleteDeck" }
);

app.MapControllerRoute(
    name: "GetUserDecks",
    pattern: "api/deck/user/{id}",
    defaults: new { controller = "Deck", action = "GetUserDecks" }
);

app.MapControllerRoute(
    name: "AddVerbToDeck",
    pattern: "api/deck/verb",
    defaults: new { controller = "Deck", action = "AddVerbToDeck" }
);


app.Run();

