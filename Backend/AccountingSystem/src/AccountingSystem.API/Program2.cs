//using AccountingSystem.Application.Intrefaces;
//using AccountingSystem.Application.Mappings;
//using AccountingSystem.Application.Handlers.Tasks;
//using AccountingSystem.Domain.Enums;
//using AccountingSystem.Domain.Interfaces;
//using AccountingSystem.Domain.Interfaces.Repositories;
//using AccountingSystem.Infrastructure.Data;
//using AccountingSystem.Infrastructure.Services;
//using AccountingSystem.Infrastructure.Repositories;
//using AutoMapper;
//using FluentValidation;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using Npgsql;
//using System.Text;

//AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

//var builder = WebApplication.CreateBuilder(args);

//// ========================================
//// 1. DATABASE & ENUM MAPPING
//// ========================================
//try
//{
//    NpgsqlConnection.GlobalTypeMapper.EnableUnmappedTypes();
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<ReportStatus>("report_status");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentMethod>("payment_method");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<TaskCategory>("task_category");
//    _logger.LogInformation("✅ Enum mapping completed successfully!");
//    foreach (var value in Enum.GetValues<AccountingSystem.Domain.Enums.TaskStatus>())
//        _logger.LogInformation($"  - TaskStatus: {value}");
//}
//catch (Exception ex)
//{
//    _logger.LogInformation($"❌ Enum mapping failed: {ex.Message}");
//}

//builder.Services.AddDbContext<AccountingDbContext>(options =>
//    options.UseNpgsql(
//        builder.Configuration.GetConnectionString("DefaultConnection"),
//        npgsqlOptions =>
//        {
//            npgsqlOptions.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
//            npgsqlOptions.MapEnum<ReportStatus>("report_status");
//            npgsqlOptions.MapEnum<PaymentMethod>("payment_method");
//            npgsqlOptions.MapEnum<TaskCategory>("task_category");
//        }));

//// ========================================
//// 2. REPOSITORIES & SERVICES
//// ========================================
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
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
//builder.Services.AddScoped<ITokenService, JwtTokenService>();

//// ========================================
//// 3. AUTOMAPPER
//// ========================================
//var mapperConfig = new MapperConfiguration(mc =>
//{
//    mc.AddProfile(new MappingProfile());
//});
//IMapper mapper = mapperConfig.CreateMapper();
//builder.Services.AddSingleton(mapper);

//// ========================================
//// 4. MediatR (CQRS)
//// ========================================
//builder.Services.AddMediatR(cfg =>
//{
//    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly);
//    cfg.RegisterServicesFromAssemblyContaining<UpdateTaskStatusCommandHandler>();
//});

//// ========================================
//// 5. FluentValidation
//// ========================================
//builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

//// ========================================
//// 6. JWT Authentication
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

//builder.Services.AddAuthorization();

//// ========================================
//// 7. Controllers, Swagger & CORS
//// ========================================
//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddOpenApi();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//    {
//        policy.WithOrigins("http://localhost:4200", "https://localhost:4200")
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//              .AllowCredentials();
//    });
//});

//// ========================================
//// 8. BUILD AP
