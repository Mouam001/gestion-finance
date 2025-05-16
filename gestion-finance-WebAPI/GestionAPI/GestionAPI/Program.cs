using System.Text;
using Business.Implementations;
using Business.Interfaces;
using DataAccess.Implementations;
using DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

// Configuration de l'authentification JWT
builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Mettre à true en production pour forcer HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true
        };
    });

// Configuration de la base de données
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=gestion.db"));

// 🔹 Ajouter les services nécessaires
builder.Services.AddControllers(); // Support des contrôleurs
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "GestionAPI", Version = "v1" });

    // Ajouter la configuration du JWT dans Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Description = "Enter 'Bearer {votre_token"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    
    // Docs
    
});

// 🔹 Configuration de l’injection de dépendances (DI)
builder.Services.AddScoped<IUserService, UserService>(); // Service métier
builder.Services.AddScoped<IUserRepository, UserRepository>(); // Accès aux données
builder.Services.AddScoped<IAuthservice, AuthService>(); // Service métier
builder.Services.AddScoped<IObpService, ObpService>();

// Ajouter HttpClient et le service OBP
builder.Services.AddHttpClient();
builder.Services.AddScoped<ObpService>();

builder.Services.AddControllers();
var app = builder.Build();

// 🔹 Activer Swagger en mode développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseHttpsRedirection(); // Ajout de redirection vers HTTPS
app.UseAuthorization();
app.MapControllers(); // 👈 Permet d'utiliser les contrôleurs

app.Run();


