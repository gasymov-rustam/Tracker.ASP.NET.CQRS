using Microsoft.OpenApi.Models;
using Tracker.Application;
using Tracker.Application.Constants;
using Tracker.Infrastructure;
using Tracker.Shared;

var builder = WebApplication.CreateBuilder(args).AddShared();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var redisConnection = builder.Configuration.GetConnectionString(TrackerApplicationConsts.REDIS_CONNECTION_STRING);

// Add services to the container.
builder.Services.AddDataBaseContext(connectionString ?? "");

// convert DateOnly to string
builder.Services.AddSwaggerGen(c => c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" }));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication(redisConnection ?? "");

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseShared();
app.MapControllers();

app.Run();
