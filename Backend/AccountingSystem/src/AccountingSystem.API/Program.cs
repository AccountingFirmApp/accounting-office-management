using AccountingSystem.API.Controller;
using AccountingSystem.API.Jobs;
using AccountingSystem.Application.Intrefaces;
using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Repositories;
using AccountingSystem.Infrastructure.Services;
using AutoMapper;
using FluentValidation;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file (for development)
// In production, use Azure App Service Configuration or Key Vault
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
        var parts = line.Split('=', 2);
        if (parts.Length == 2)
        {
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
        }
    }
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ========================================
// Database + ENUM mapping
// ========================================
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Database connection string not configured. Set DB_CONNECTION_STRING environment variable.");

var nullNameTranslator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();

builder.Services.AddDbContext<AccountingDbContext>(options =>
{
    options.UseNpgsql(
        connectionString,
        npgsqlOptions =>
        {
            npgsqlOptions.MapEnum<TaskStatus1>("TaskStatus1", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<TaskCategory>("task_category", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<ReportStatus>("report_status", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<PaymentMethod>("payment_method", nameTranslator: nullNameTranslator);
        });
});


var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

// מיפוי כל ה-ENUMs
dataSourceBuilder.MapEnum<AccountingSystem.Domain.Enums.TaskStatus1>("task_status");
dataSourceBuilder.MapEnum<ReportStatus>("report_status");
dataSourceBuilder.MapEnum<PaymentMethod>("payment_method");
dataSourceBuilder.MapEnum<TaskCategory>("task_category");

var dataSource = dataSourceBuilder.Build();

builder.Services.AddDbContext<AccountingDbContext>(options =>
    options.UseNpgsql(dataSource));
// ========================================
// Hangfire
// ========================================
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(connectionString)
);

builder.Services.AddHangfireServer();

// ========================================
// Dependency Injection
// ========================================
builder.Services.AddScoped<ReportGenerationJob>();

builder.Services.AddScoped<IReportInstanceRepository, ReportInstanceRepository>();
builder.Services.AddScoped<IAccountingFirmRepository, AccountingFirmRepository>();
builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IWorkerRepository, WorkerRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<ICompanyContactRepository, CompanyContactRepository>();
builder.Services.AddScoped<ICompanyWorkerRepository, CompanyWorkerRepository>();
builder.Services.AddScoped<IReportTypeRepository, ReportTypeRepository>();
builder.Services.AddScoped<IFrequencyRepository, FrequencyRepository>();
builder.Services.AddScoped<ICompanyreportconfigRepository, CompanyReportConfigRepository>();
builder.Services.AddScoped<ITaskTypeRepository, TaskTypeRepository>();
builder.Services.AddScoped<ICompanyTaskRepository, CompanyTaskRepository>();
builder.Services.AddScoped<IWorkerRoleTypeRepository, WorkerRoleTypeRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// ========================================
// AutoMapper
// ========================================
var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});
builder.Services.AddSingleton(mapperConfig.CreateMapper());

// ========================================
// MediatR + Validation
// ========================================
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

// ========================================
// JWT Authentication
// ========================================
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? builder.Configuration.GetSection("Jwt")["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey not configured. Set JWT_SECRET_KEY environment variable.");

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER")
    ?? builder.Configuration.GetSection("Jwt")["Issuer"]
    ?? "AccountingSystem.API";

var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE")
    ?? builder.Configuration.GetSection("Jwt")["Audience"]
    ?? "AccountingSystem.Client";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        ValidAudience = jwtAudience,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtSecretKey)
        ),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// ========================================
// Controllers + Swagger
// ========================================
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Accounting API",
        Version = "v1"
    });

    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
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
            Array.Empty<string>()
        }
    });
});

// ========================================
// CORS
// ========================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();
app.UseHangfireDashboard();

// ========================================
// ========================================
RecurringJob.AddOrUpdate<ReportGenerationJob>(
    "monthly-report-25",
    job => job.RunMonthlyReport(),
   "0 1 25 * *"

);
RecurringJob.AddOrUpdate<CheckReportGenerationJob>(
    "check-report-generation",
    job => job.RunDailyCheckReport(),
   Cron.Daily());
    

// ========================================
// HTTP Pipeline
// ========================================
app.UseCors("AllowAngular");

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
