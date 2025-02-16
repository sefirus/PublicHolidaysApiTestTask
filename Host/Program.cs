using System.Text.Json.Serialization;
using Core.Interfaces;
using DataAccess;
using DataAccess.Repositories;
using Microsoft.EntityFrameworkCore;
using PublicHolidaysApiTestTask;
using WebApi.Controllers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

builder.Services.AddDbContext<PublicHolidaysDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Services
    .AddControllers(options =>
    {
        options.SuppressAsyncSuffixInActionNames = false;
    })
    .AddJsonOptions(jsonOptions =>
    {
        jsonOptions.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    })
    .AddApplicationPart(typeof(HolidayScheduleController).Assembly);

builder.Services.AddTransient(typeof(IRepository<>), typeof(Repository<>));

builder.Services.AddHttpClient();

builder.Services.AddApplicationServices();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
