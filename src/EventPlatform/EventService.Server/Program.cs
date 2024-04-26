using EventService.Server.Core.Entities;
using EventService.Server.Persistence;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Common;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json")
    .AddEnvironmentVariables();
    

// Add services to the container.
builder.Services.AddScoped<IDbContext>(ctx => new DbContext(new DatabaseConfiguration 
{
    ConnectionString = builder.Configuration["ConStr-MongoDB"] ?? throw new Exception("No DB-Connectionstring given!"),
    DatabaseName = "EventServiceDB"
}));

builder.Services.AddScoped<IEventRepository, EventRepository>();
builder.Services.AddScoped<IEventSubscriptionRepository, EventSubscriptionRepository>();
builder.Services.AddKeycloakAuthentication(new KeycloakAuthenticationOptions
{
    SslRequired = "none",
    Realm = "EventPlatform",
    AuthServerUrl = "http://keycloak:8080/",
    Resource = "eventplatform-client",
    RolesSource = RolesClaimTransformationSource.Realm
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var openIdConnectUrl = $"{builder.Configuration["Keycloak:auth-server-url"]}"
 + $"realms/{builder.Configuration["Keycloak:realm"]}/"
 + ".well-known/openid-configuration";

builder.Services.AddSwaggerGen(c =>
{
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.OpenIdConnect,
        OpenIdConnectUrl = new Uri(openIdConnectUrl),
        Scheme = "bearer",
        BearerFormat = "JWT",
        Reference = new OpenApiReference
        {
            Id = "Bearer",
            Type = ReferenceType.SecurityScheme
        }
    };
    c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {securityScheme, Array.Empty<string>()}
    });
});

var app = builder.Build();

app.UseCors(x => x
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowAnyOrigin());

app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

using IServiceScope scope = app.Services.CreateScope();
IServiceProvider serviceProvider = scope.ServiceProvider;
var context = serviceProvider.GetRequiredService<IDbContext>();
var repo = serviceProvider.GetService<IEventRepository>();

await repo!.AddMany(Enumerable.Range(1, 100).Select(index => new Event
{
    StartDate = DateTime.Now.AddDays(index),
    EndDate = DateTime.Now.AddDays(index).AddHours(5),
    Description = "Das ist ein Testevent",
    Name = "Event_" + index
}).ToArray());

await context.SaveChanges();

app.Run();
