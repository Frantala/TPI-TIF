using System.Text;
using Application.Services;
using Application.Interfaces;
using Domain.Interfaces;
using Infrastructure.Persistance;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi;
using Microsoft.IdentityModel.Tokens;
using NSwag.AspNetCore;

namespace Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException(
                    "No se encontró la cadena de conexión 'DefaultConnection'.");

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserServices, UserService>();

            var jwtKey = builder.Configuration["JwtSettings:SecretKey"]
                ?? throw new InvalidOperationException(
                    "Falta configurar JwtSettings:SecretKey.");
            if (Encoding.UTF8.GetByteCount(jwtKey) < 32)
            {
                throw new InvalidOperationException(
                    "JwtSettings:SecretKey debe tener al menos 32 bytes.");
            }

            var issuer = builder.Configuration["JwtSettings:Issuer"]
                ?? throw new InvalidOperationException(
                    "Falta configurar JwtSettings:Issuer.");
            var audience = builder.Configuration["JwtSettings:Audience"]
                ?? throw new InvalidOperationException(
                    "Falta configurar JwtSettings:Audience.");
            builder.Services.AddScoped<IJwtService>(_ =>
                new JwtService(jwtKey, issuer, audience));

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
            {
                options.AddDocumentTransformer((document, _, _) =>
                {
                    var components = document.Components;
                    if (components is null)
                    {
                        components = new OpenApiComponents();
                        document.Components = components;
                    }

                    components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
                    components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        BearerFormat = "JWT"
                    });

                    var securityRequirements = document.Security;
                    if (securityRequirements is null)
                    {
                        securityRequirements = [];
                        document.Security = securityRequirements;
                    }

                    securityRequirements.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] = []
                    });

                    return Task.CompletedTask;
                });
            });

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = issuer,
                        ValidateAudience = true,
                        ValidAudience = audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtKey)),
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.FromSeconds(30)
                    };
                });

            builder.Services.AddAuthorization();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUi(settings =>
                    settings.SwaggerRoutes.Add(
                        new SwaggerUiRoute("TPI-TIF API v1", "/openapi/v1.json")));
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
