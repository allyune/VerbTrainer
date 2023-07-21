using VerbTrainer.Data;
using Microsoft.EntityFrameworkCore;
using VerbTrainer.DbInitializer;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<DbInitializer>();
builder.Services.AddDbContext<VerbTrainerDbContext>(options =>
options.UseNpgsql(builder.Configuration.GetConnectionString("VerbTrainerConnectionString")));

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
        name: "verbConjugations",
        pattern: "{controller=Home}/{action=GetVerbConjugations}/{id?}");

app.Run();

