using Blazored.LocalStorage;
using GestionWebAPP.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Blazorise;
using Blazorise.Bootstrap5;
using Blazorise.Icons.FontAwesome;
using Business.Interfaces;
using GestionWebAPP.Services;
using Microsoft.AspNetCore.Components.Authorization;



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

// Ajout du HttpClient pour la communication avec l'API
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5120/") });

// Enregistrement de CustomAuthenticationStateProvider


builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<TransactionApiService>();
builder.Services.AddScoped<AuthenticationStateProvider, AuthStateProvider>();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<UserApiService>();

builder.Services.AddScoped<TransactionApiService>();
builder.Services.AddScoped(sp =>
{
    var client = new HttpClient { BaseAddress = new Uri("http://localhost:5120/") };
    return client;
});



// Enregistrement des services AuthService


// Enregistrement de Blazored.LocalStorage
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();


var app = builder.Build(); // Doit être appelé avant toute configuration de pipeline

// Configuration du pipeline de requêtes HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

await app.RunAsync(); // RunAsync doit être exécuté après la configuration des middlewares
