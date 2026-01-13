using AccountingSystem.Application.Intrefaces;  // ⬅️ הוסף!
using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Services;  // ⬅️ הוסף!
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;  // ⬅️ הוסף!
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;  // ⬅️ הוסף!
using System.Text;  // ⬅️ הוסף!

var builder = WebApplication.CreateBuilder(args);

// ========================================
// 1. Database
// ========================================
builder.Services.AddDbContext<AccountingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================================
// 2. AutoMapper
// ========================================
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// ========================================
// 3. Dependency Injection
// ========================================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ⬇️ הוסף את השירותים החדשים!
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// ========================================
// 4. MediatR (CQRS)
// ========================================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

// ========================================
// 5. FluentValidation
// ========================================
builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

// ========================================
// 6. JWT Authentication ⬅️ חדש!
// ========================================
var jwtSettings = builder.Configuration.GetSection("Jwt");
var secretKey = jwtSettings["SecretKey"] ??
    throw new InvalidOperationException("JWT SecretKey לא מוגדר ב-appsettings.json");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();  // ⬅️ הוסף!

// ========================================
// 7. Controllers & API
// ========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

// ========================================
// 8. CORS for Angular
// ========================================
// ========================================
// 8. CORS for Angular
// ========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins(
                "http://localhost:4200",
                "https://localhost:4200"
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});
var app = builder.Build();

// ========================================
// HTTP Pipeline Configuration
// ========================================

// CORS
app.UseCors("AllowAngular");

// Development tools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1");
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();   
app.MapControllers();

app.Run();