using System.Net;
using Microsoft.OpenApi.Models;
using Tracker.Application;
using Tracker.Infrastructure;
using Tracker.Shared;

var builder = WebApplication.CreateBuilder(args).AddShared();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
// Add services to the container.
builder.Services.AddDataBaseContext(connectionString ?? "");

// convert DateOnly to string
builder.Services.AddSwaggerGen(c => c.MapType<DateOnly>(() => new OpenApiSchema { Type = "string", Format = "date" }));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddApplication();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.UseShared();
app.MapControllers();

app.Run();
