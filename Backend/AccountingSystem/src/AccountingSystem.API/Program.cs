

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
using AccountingSystem.Application.Handlers.Tasks;
using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

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

// ================================
// MIDDLEWARE
// ================================
app.UseCors("AllowAngular"); // ← רק פעם אחת!

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1"));
}

app.UseHttpsRedirection();
app.MapControllers();

Console.WriteLine("🚀 Server is running...");

app.Run();