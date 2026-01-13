

//using AccountingSystem.Application.Handlers.Tasks;
//using AccountingSystem.Application.Mappings;
//using AccountingSystem.Domain.Enums;
//using AccountingSystem.Domain.Interfaces;
//using AccountingSystem.Infrastructure.Data;
//using FluentValidation;
//using Microsoft.EntityFrameworkCore;
//using Npgsql;

//var builder = WebApplication.CreateBuilder(args);

//// ================================
//// MAPPING ENUMS - ONLY GLOBAL
//// ================================
//try
//{
//    NpgsqlConnection.GlobalTypeMapper.EnableUnmappedTypes();

//    NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<ReportStatus>("report_status");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentMethod>("payment_method");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<TaskCategory>("task_category");

//    Console.WriteLine("✅ Enum mapping completed successfully!");
//    foreach (var value in Enum.GetValues<AccountingSystem.Domain.Enums.TaskStatus>())
//        Console.WriteLine($"  - TaskStatus: {value}");
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"❌ Enum mapping failed: {ex.Message}");
//}

//// ================================
//// SERVICES
//// ================================
//builder.Services.AddOpenApi();
//builder.Services.AddAutoMapper(typeof(MappingProfile));
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
//builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));
//builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();
//builder.Services.AddControllers();
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//        policy.WithOrigins("http://localhost:4200")
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//              .AllowCredentials());
//});

//// ================================
//// DATABASE
//// ================================
////builder.Services.AddDbContext<AccountingDbContext>(options =>
////{
////    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
////});
//builder.Services.AddDbContext<AccountingDbContext>(options =>
//{
//    options.UseNpgsql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        npgsqlOptions =>
//        {
//            npgsqlOptions.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
//            npgsqlOptions.MapEnum<ReportStatus>("report_status");
//            npgsqlOptions.MapEnum<PaymentMethod>("payment_method");
//            npgsqlOptions.MapEnum<TaskCategory>("task_category");
//        });
//});
//// הוסף את MediatR
//builder.Services.AddMediatR(cfg => {
//    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
//    cfg.RegisterServicesFromAssemblyContaining<UpdateTaskStatusCommandHandler>();  // ← וודא שזה כאן!
//});


//var app = builder.Build();

//app.UseCors("AllowAngular");

//app.UseCors("AllowAngular");

//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();
//    app.UseSwaggerUI(options =>
//        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1"));
//}

//app.UseHttpsRedirection();
//app.MapControllers();
//app.Run();



using AccountingSystem.Application.Intrefaces;  // ⬅️ הוסף!
using AccountingSystem.Application.Mappings;
using AccountingSystem.Application.Handlers.Tasks;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Services;  // ⬅️ הוסף!
using AutoMapper;
using AccountingSystem.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;  // ⬅️ הוסף!
using Microsoft.EntityFrameworkCore;
using Npgsql;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ========================================
// 1. Database
// ========================================
builder.Services.AddDbContext<AccountingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ✅ רישום כל הRepositories (הוסף את זה!)
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

// DI - UnitOfWork
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
// ================================
// MAPPING ENUMS - ONLY GLOBAL
// ================================
try
{
    NpgsqlConnection.GlobalTypeMapper.EnableUnmappedTypes();
    NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
    NpgsqlConnection.GlobalTypeMapper.MapEnum<ReportStatus>("report_status");
    NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentMethod>("payment_method");
    NpgsqlConnection.GlobalTypeMapper.MapEnum<TaskCategory>("task_category");
    Console.WriteLine("✅ Enum mapping completed successfully!");
    foreach (var value in Enum.GetValues<AccountingSystem.Domain.Enums.TaskStatus>())
        Console.WriteLine($"  - TaskStatus: {value}");
}
catch (Exception ex)
{
    Console.WriteLine($"❌ Enum mapping failed: {ex.Message}");
}

// ================================
// DATABASE
// ================================
builder.Services.AddDbContext<AccountingDbContext>(options =>
{
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsqlOptions =>
        {
            npgsqlOptions.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
            npgsqlOptions.MapEnum<ReportStatus>("report_status");
            npgsqlOptions.MapEnum<PaymentMethod>("payment_method");
            npgsqlOptions.MapEnum<TaskCategory>("task_category");
        });
});

// ================================
// SERVICES
// ================================
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// ← רק רישום אחד של MediatR! זה הכי חשוב!
builder.Services.AddMediatR(cfg => {
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly); // API Assembly
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly); // Application Assembly
    cfg.RegisterServicesFromAssemblyContaining<UpdateTaskStatusCommandHandler>(); // עוד פעם Application Assembly
});

builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials());
});

var app = builder.Build();

// ========================================
// HTTP Pipeline Configuration
// ========================================
// CORS
// ================================
// MIDDLEWARE
// ================================
app.UseCors("AllowAngular"); // ← רק פעם אחת!

// Development tools
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1"));
}

app.UseHttpsRedirection();

app.UseAuthentication();  
app.UseAuthorization();   
app.MapControllers();

Console.WriteLine("🚀 Server is running...");

app.Run();