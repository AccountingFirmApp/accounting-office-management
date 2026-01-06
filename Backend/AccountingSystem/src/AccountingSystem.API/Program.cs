//using AccountingSystem.Application.Mappings;
//using AccountingSystem.Domain.Enums;
//using AccountingSystem.Domain.Interfaces;
//using AccountingSystem.Infrastructure.Data;
//using FluentValidation;
//using Microsoft.EntityFrameworkCore;

//using Npgsql;

//var builder = WebApplication.CreateBuilder(args);
//try
//{
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<AccountingSystem.Domain.Enums.TaskStatus>("task_status");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<ReportStatus>("report_status");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<PaymentMethod>("payment_method");
//    NpgsqlConnection.GlobalTypeMapper.MapEnum<TaskCategory>("task_category");

//    Console.WriteLine("✅ Enum mapping completed successfully!");

//    // הדפס את הערכים של TaskStatus כדי לוודא
//    foreach (var value in Enum.GetValues<AccountingSystem.Domain.Enums.TaskStatus>())
//    {
//        Console.WriteLine($"  - TaskStatus: {value}");
//    }
//}
//catch (Exception ex)
//{
//    Console.WriteLine($"❌ Enum mapping failed: {ex.Message}");
//}



//// Add services to the container.
//builder.Services.AddOpenApi();

//// AUTO MAPPING
//builder.Services.AddAutoMapper(typeof(MappingProfile));

////Database Configuration
////builder.Services.AddDbContext<AccountingDbContext>(options =>
////    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
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
using AccountingSystem.Domain.Enums;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Npgsql;

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
// SERVICES
// ================================
builder.Services.AddOpenApi();
builder.Services.AddAutoMapper(typeof(MappingProfile));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));
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

// ================================
// DATABASE
// ================================
//builder.Services.AddDbContext<AccountingDbContext>(options =>
//{
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//});
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



var app = builder.Build();

app.UseCors("AllowAngular");

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Accounting API V1"));
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();
