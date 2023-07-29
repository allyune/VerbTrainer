using VerbTrainer.Data;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.DbInitializer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using VerbTrainer.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<DbInitializer>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("JwtSettings", options))
    .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme,
        options => builder.Configuration.Bind("CookieSettings", options));
builder.Services.AddDbContext<VerbTrainerDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("VerbTrainerConnectionString")));
builder.Services.AddScoped<IPasswordHashService, PasswordHashService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var services = scope.ServiceProvider;

	var initialiser = services.GetRequiredService<DbInitializer>();

	initialiser.Run();
}

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

app.MapControllerRoute(
    name: "RegisterUser",
    pattern: "api/auth/register",
    defaults: new { controller = "Auth", action = "RegisterUser" }
);


app.Run();

