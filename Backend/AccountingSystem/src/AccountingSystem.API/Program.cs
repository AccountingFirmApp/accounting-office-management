



//using AccountingSystem.Application.Intrefaces;  
//using AccountingSystem.Application.Mappings;
//using AccountingSystem.Domain.Interfaces;
//using AccountingSystem.Domain.Interfaces.Repositories;
//using AccountingSystem.Infrastructure.Data;
//using AccountingSystem.Infrastructure.Services;  // ⬅️ הוסף!
//using AutoMapper;
//using AccountingSystem.Infrastructure.Repositories;
//using FluentValidation;
//using Microsoft.AspNetCore.Authentication.JwtBearer;  // ⬅️ הוסף!
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;  // ⬅️ הוסף!
//using System.Text;  // ⬅️ הוסף!
//using Microsoft.OpenApi.Models;

//var builder = WebApplication.CreateBuilder(args);
//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//// ========================================
//// 1. Database
//// ========================================
//builder.Services.AddDbContext<AccountingDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// ✅ רישום כל הRepositories (הוסף את זה!)
//builder.Services.AddScoped<IReportInstanceRepository, ReportInstanceRepository>();
//builder.Services.AddScoped<IAccountingFirmRepository, AccountingFirmRepository>();
//builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
//builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
//builder.Services.AddScoped<IRoleRepository, RoleRepository>();
//builder.Services.AddScoped<ICompanyContactRepository, CompanyContactRepository>();
//builder.Services.AddScoped<ICompanyWorkerRepository, CompanyWorkerRepository>();
//builder.Services.AddScoped<IReportTypeRepository, ReportTypeRepository>();
//builder.Services.AddScoped<IFrequencyRepository, FrequencyRepository>();
//builder.Services.AddScoped<ICompanyReportConfigRepository, CompanyReportConfigRepository>();
//builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
//builder.Services.AddScoped<ITaskRepository, TaskRepository>();
//builder.Services.AddScoped<IWorkerRoleTypeRepository, WorkerRoleTypeRepository>();
//builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

//// DI - UnitOfWork
//// ========================================
//// 2. AutoMapper
//// ========================================
//var mapperConfig = new MapperConfiguration(mc =>
//{
//    mc.AddProfile(new MappingProfile());
//});
//IMapper mapper = mapperConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);

//// ========================================
//// 3. Dependency Injection
//// ========================================
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//// ⬇️ הוסף את השירותים החדשים!
//builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
//builder.Services.AddScoped<ITokenService, JwtTokenService>();

//// ========================================
//// 4. MediatR (CQRS)
//// ========================================
//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

//// ========================================
//// 5. FluentValidation
//// ========================================
//builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

//// ========================================
//// 6. JWT Authentication ⬅️ חדש!
//// ========================================
//var jwtSettings = builder.Configuration.GetSection("Jwt");
//var secretKey = jwtSettings["SecretKey"] ??
//    throw new InvalidOperationException("JWT SecretKey לא מוגדר ב-appsettings.json");

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = jwtSettings["Issuer"],
//        ValidAudience = jwtSettings["Audience"],
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
//        ClockSkew = TimeSpan.Zero
//    };
//});

////builder.Services.AddAuthorization();  // ⬅️ הוסף!
//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminOnly", policy =>
//        policy.RequireRole("Admin"));
//});

//// ========================================
//// 7. Controllers & API
//// ========================================
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddOpenApi();

//// ========================================
//// 8. CORS for Angular
//// ========================================
//// ========================================
//// 8. CORS for Angular
//// ========================================
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//    {
//        policy.WithOrigins(
//                "http://localhost:4200",
//                "https://localhost:4200"
//            )
//            .AllowAnyMethod()
//            .AllowAnyHeader()
//            .AllowCredentials();
//    });
//});
//var app = builder.Build();

//// ========================================
//// HTTP Pipeline Configuration
//// ========================================
//// CORS
//app.UseCors("AllowAngular");

//// Development tools
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1");
//    });
//}

//app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();
//app.MapControllers();
//app.Run();


using AccountingSystem.Application.Intrefaces;
using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Services;
using AutoMapper;
using AccountingSystem.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models; // ← הוסף את זה!
using System.Text;

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ========================================
// 1. Database
// ========================================
builder.Services.AddDbContext<AccountingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ========================================
// 2. Repositories
// ========================================
builder.Services.AddScoped<IReportInstanceRepository, ReportInstanceRepository>();
builder.Services.AddScoped<IAccountingFirmRepository, AccountingFirmRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICompanyContactRepository, CompanyContactRepository>();
builder.Services.AddScoped<ICompanyWorkerRepository, CompanyWorkerRepository>();
builder.Services.AddScoped<IReportTypeRepository, ReportTypeRepository>();
builder.Services.AddScoped<IFrequencyRepository, FrequencyRepository>();
builder.Services.AddScoped<ICompanyReportConfigRepository, CompanyReportConfigRepository>();
builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IWorkerRoleTypeRepository, WorkerRoleTypeRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

// ========================================
// 3. AutoMapper
// ========================================
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);

// ========================================
// 4. Dependency Injection
// ========================================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// ========================================
// 5. MediatR (CQRS)
// ========================================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

// ========================================
// 6. FluentValidation
// ========================================
builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

// ========================================
// 7. JWT Authentication
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

//builder.Services.AddAuthorization();
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});


// ========================================
// 8. Controllers & API
// ========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// 🔥 החלפנו AddOpenApi ב-AddSwaggerGen עם JWT
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "Accounting API",
        Description = "API למערכת ניהול הנהלת חשבונות"
    });

    // הוספת JWT Authentication ל-Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "הזן JWT Token בפורמט: Bearer {token}"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
});

// ========================================
// 9. CORS for Angular
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
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // ← שינוי
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounting API V1"); // ← שינוי
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
