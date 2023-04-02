using Tracker.Application;
using Tracker.Application.Common.Interfaces;
using Tracker.Infrastructure;
using Tracker.Infrastructure.Dal;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("postgresConnection");
// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddDataBaseContext(connectionString ?? "");
builder.Services.AddScoped<ITrackerDBContext, TrackerDbContext>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();
//add cashing
app.UseResponseCaching();

app.Run();
