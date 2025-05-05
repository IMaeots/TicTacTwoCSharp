using Common;
using Data.Context;
using Data.Repositories;
using Data.Repositories.Config;
using Data.Repositories.Game;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

Directory.CreateDirectory(Constants.DatabaseDirectory);
Directory.CreateDirectory(Constants.GamesPath);
Directory.CreateDirectory(Constants.ConfigurationsPath);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
connectionString = connectionString.Replace("<%location%>", Constants.DatabaseDirectory);

builder.Services.AddDbContext<GameDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();

var useDatabase = builder.Configuration.GetValue<bool>("UseDatabase", true);
if (useDatabase)
{
    builder.Services.AddScoped<IGameRepository, GameRepositoryDb>();
    builder.Services.AddScoped<IConfigRepository, ConfigRepositoryDb>();
}
else
{
    builder.Services.AddScoped<IGameRepository, GameRepositoryJson>();
    builder.Services.AddScoped<IConfigRepository, ConfigRepositoryJson>();
}

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

if (useDatabase)
{
    try
    {
        using var scope = app.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GameDbContext>();
        dbContext.Database.Migrate();
        Console.WriteLine("Database migration completed successfully");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error during database migration: {ex.Message}");
    }
}

app.Run();
