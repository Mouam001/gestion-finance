using GestionWebAPP.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DbContext = DataAccess.DbContext;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;

var builder = WebApplication.CreateBuilder(args);

// Configuration de la base de données avec SQLite
builder.Services.AddDbContext<DbContext>(options =>
    SqliteDbContextOptionsBuilderExtensions.UseSqlite(options));  // Utilise ta chaîne de connexion

// Configuration d'Identity
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<DbContext>()
    .AddDefaultTokenProviders();

// Ajout des services Blazorise
builder.Services.AddBlazorise(options =>
    {
        options.Immediate = true;
    })
    .AddBootstrap5Providers()
    .AddFontAwesomeIcons();

// Ajout des composants Razor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5120/") });


var app = builder.Build();

// Configuration du pipeline de requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();