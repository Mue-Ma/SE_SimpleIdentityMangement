using EventService.Server.Core.Entities;
using EventService.Server.Persistence;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables(); 

// Add services to the container.
builder.Services.AddScoped<IDbContext>(ctx => new DbContext(new DatabaseConfiguration 
{
    ConnectionString = builder.Configuration["ConStr-MongoDB"] ?? throw new Exception("No DB-Connectionstring given!"),
    DatabaseName = "EventServiceDB"
}));
builder.Services.AddScoped<IEventRepository, EventRepository>();

builder.Services.AddControllers();
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

app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowAnyOrigin());

app.MapControllers();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider serviceProvider = scope.ServiceProvider;
var context = serviceProvider.GetRequiredService<IDbContext>();
var repo = serviceProvider.GetService<IEventRepository>();

await repo!.AddMany(Enumerable.Range(1, 100).Select(index => new WeatherForecast
{
    Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    TemperatureC = Random.Shared.Next(-20, 55),
    Summary = "Nice"
}).ToArray());

await context.SaveChanges();

app.Run();
