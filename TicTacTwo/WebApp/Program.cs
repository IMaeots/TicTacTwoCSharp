using Common;
using Data.Context;
using Data.Repositories;
using Data.Repositories.Config;
using Data.Repositories.Game;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
                       throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
connectionString = connectionString.Replace("<%location%>", Constants.DatabaseDirectory);

builder.Services.AddDbContext<GameDbContext>(options => options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddRazorPages();

const bool useDatabase = true;
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
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
    .WithStaticAssets();

app.Run();
