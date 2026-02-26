using AccountingSystem.Application.Intrefaces;
using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Jobs;
using AccountingSystem.Infrastructure.Repositories;
using AccountingSystem.Infrastructure.Services;
using AutoMapper;
using AccountingSystem.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AccountingSystem.Domain.Enums;
using Npgsql;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables from .env file (for development)
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", ".env");
if (File.Exists(envPath))
{
    foreach (var line in File.ReadAllLines(envPath))
    {
        if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#")) continue;
        var parts = line.Split('=', 2);
        if (parts.Length == 2)
            Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim());
    }
}

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ========================================
// 1. Database with ENUMs Mapping ⭐
// ========================================
// Create a single instance of the name translator (singleton pattern)
var nullNameTranslator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();


// ========================================
// Database + ENUM mapping
// ========================================

// 1. הגדרת ה-DataSourceBuilder ומיפוי ה-Enums
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);

// הוספת השורה הזו פותרת את בעיית ה-InvalidCast ברוב המקרים
dataSourceBuilder.EnableUnmappedTypes();

// מיפוי ה-Enums - שים לב לשם המדויק ב-DB (ב-DbContext כתבת TaskStatus1)
dataSourceBuilder.MapEnum<TaskStatus1>("TaskStatus1"); // השם כפי שהוא מופיע ב-HasPostgresEnum ב-DbContext
dataSourceBuilder.MapEnum<ReportStatus>("report_status");
dataSourceBuilder.MapEnum<PaymentMethod>("payment_method");
dataSourceBuilder.MapEnum<TaskCategory>("task_category");

var dataSource = dataSourceBuilder.Build();

// 2. רישום ה-DbContext
builder.Services.AddDbContext<AccountingDbContext>(options =>
    options.UseNpgsql(dataSource));
// למחוק/להסיר את הקטע של ה-AddDbContext השני שהיה כאן!
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING")
    ?? builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Database connection string not configured.");

var nullNameTranslator = new Npgsql.NameTranslation.NpgsqlNullNameTranslator();

builder.Services.AddDbContext<AccountingDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            // Map enums WITHOUT name transformation (keep PascalCase)
            // Reuse the same translator instance to avoid creating multiple ServiceProviders
            npgsqlOptions.MapEnum<TaskStatus1>("TaskStatus1", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<TaskCategory>("task_category", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<ReportStatus>("report_status", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<PaymentMethod>("payment_method", nameTranslator: nullNameTranslator);
            npgsqlOptions.MapEnum<RecurrenceType>("recurrence_type", nameTranslator: nullNameTranslator);
        });
});

// ========================================
// 2. Repositories
// ========================================
builder.Services.AddHangfire(config => config.UsePostgreSqlStorage(connectionString));
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
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>(); // ✅
builder.Services.AddHttpContextAccessor(); // ✅ חשוב!

builder.Services.AddScoped<IChecklistItemRepository, ChecklistItemRepository>();
builder.Services.AddScoped<ITaskConfigurationRepository, TaskConfigurationRepository>();
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
// 4. Dependency Injection
// ========================================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

// ========================================
// 5. MediatR (CQRS)
// ========================================
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));
builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

// ========================================
// 7. JWT Authentication
// ========================================
var jwtSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
    ?? builder.Configuration.GetSection("Jwt")["SecretKey"]
    ?? throw new InvalidOperationException("JWT SecretKey not configured.");

var jwtIssuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? builder.Configuration.GetSection("Jwt")["Issuer"] ?? "AccountingSystem.API";
var jwtAudience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? builder.Configuration.GetSection("Jwt")["Audience"] ?? "AccountingSystem.Client";

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
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecretKey)),
            ClockSkew = TimeSpan.Zero
        };
    });

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
builder.Services.AddHostedService<AutomaticTaskGenerationJob>();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "Accounting API", Version = "v1" });
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
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
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
// Recurring Jobs
// ========================================
RecurringJob.AddOrUpdate<ReportGenerationJob>("monthly-report-25", job => job.RunMonthlyReport(), "0 1 25 * *");
BackgroundJob.Schedule<CheckReportGenerationJob>(
    job => job.RunDailyCheckReport(),
    TimeSpan.FromMinutes(1)
);
// ========================================
// HTTP Pipeline
// ========================================
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Accounting API V1");
    });
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();