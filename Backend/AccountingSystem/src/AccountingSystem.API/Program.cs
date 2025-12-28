using AccountingSystem.Application.Mappings;
using AccountingSystem.Domain.Interfaces;
using AccountingSystem.Infrastructure.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
//AUTO MAPPING-שימוש ב-
builder.Services.AddAutoMapper(typeof(MappingProfile));
//Database Configuration
builder.Services.AddDbContext<AccountingDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
//DI
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
//MediatR (CQRS)
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(typeof(MappingProfile).Assembly));
// FluentValidation
builder.Services.AddValidatorsFromAssemblyContaining<MappingProfile>();
//  Controllers
builder.Services.AddControllers();
//  CORS for Angular
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
//CORS
app.UseCors("AllowAngular");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();


app.Run();


