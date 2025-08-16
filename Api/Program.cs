using Application.Ports;
using Application.Services;
using Infrastructure.Persistence;
using Infrastructure.Repositories;
using Infrastructure.Services; // AuthService
using Microsoft.EntityFrameworkCore;

// 🔽 JWT & Swagger
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using Api.Services;

var builder = WebApplication.CreateBuilder(args);

// 1) DbContext (SQL Server apuntando a InventarioDB)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// 2) Repositorios y servicios
builder.Services.AddScoped<IUsuarioRepository, UsuarioRepository>();
builder.Services.AddScoped<IAuthService, AuthService>(); // ✅ Usa Usuario/Rol reales
builder.Services.AddScoped<ITokenService, TokenService>(); // ✅ Genera JWT
builder.Services.AddScoped<IProductRepository, ProductRepository>(); // ✅ Registro de repositorio de productos
builder.Services.AddScoped<IWishlistRepository, WishlistRepository>();
// 3) Controllers + Swagger (con JWT)
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.SwaggerDoc("v1", new OpenApiInfo { Title = "Inventario API", Version = "v1" });

    // 🔐 Soporte para "Authorize" con Bearer en Swagger
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Usa: Bearer {tu_token}"
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
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
            Array.Empty<string>()
        }
    });
});

// 4) CORS (Angular en 4200, React en 3000, agregar producción)
// builder.Services.AddCors(options =>
// {
//     options.AddPolicy("ng", policy =>
//         policy.WithOrigins(
//             "http://localhost:4200",
//             "http://localhost:3000"
//             // agregar aquí dominio en producción
//         )
//         .AllowAnyHeader()
//         .AllowAnyMethod());
// });

builder.Services.AddCors(options =>
{
    options.AddPolicy("ng", policy =>
        policy
            .AllowAnyOrigin()   // ✅ permite cualquier origen
            .AllowAnyHeader()
            .AllowAnyMethod());
});


// 5) JWT Auth
var jwt = builder.Configuration.GetSection("Jwt");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = key
        };
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Pipeline
app.UseCors("ng");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
