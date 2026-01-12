//using AccountingSystem.Application.Mappings;
//using AccountingSystem.Domain.Interfaces;
//using AccountingSystem.Infrastructure.Data;
//using FluentValidation;
//using Microsoft.EntityFrameworkCore;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddOpenApi();

//// AUTO MAPPING
//builder.Services.AddAutoMapper(typeof(MappingProfile));

//// Database Configuration
//builder.Services.AddDbContext<AccountingDbContext>(options =>
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

//// DI
//builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

//// MediatR (CQRS)
//builder.Services.AddMediatR(cfg =>
//    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

//// FluentValidation
//builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

//// Controllers
//builder.Services.AddControllers();

//// CORS for Angular
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("AllowAngular", policy =>
//    {
//        policy.WithOrigins("http://localhost:4200")
//              .AllowAnyMethod()
//              .AllowAnyHeader()
//              .AllowCredentials();
//    });
//});

//var app = builder.Build();

//// CORS
//app.UseCors("AllowAngular");

//// Configure the HTTP request pipeline.
//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.MapOpenApi();

//    // הוסף Swagger UI (אופציונלי אבל מומלץ מאוד)
//    app.UseSwaggerUI(options =>
//    {
//        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1");
//    });
//}

//app.UseHttpsRedirection();

//// ✅ זו השורה החסרה - חייבת להיות לפני app.Run()!
//app.MapControllers();

//app.Run();



using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Domain.Interfaces.Repositories;
using AccountingSystem.Infrastructure.Data;
using AccountingSystem.Infrastructure.Repositories;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();

// AUTO MAPPING
builder.Services.AddAutoMapper(typeof(MappingProfile));

// Database Configuration
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
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// MediatR (CQRS)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));

// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();

// Controllers
builder.Services.AddControllers();

// CORS for Angular
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

var app = builder.Build();
// CORS
app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1");
    });
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();