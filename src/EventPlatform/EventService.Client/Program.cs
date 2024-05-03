using EventPlatform.Common.Core.Factories;
using EventService.Client.Handlers;
using EventService.Client.Services;
using EventService.Client.Services.Contracts;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<EventService.Client.App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddOidcAuthentication(options =>
{
    options.ProviderOptions.MetadataUrl = "http://localhost/realms/EventPlatform/.well-known/openid-configuration";
    options.ProviderOptions.Authority = "http://localhost/realms/EventPlatform";

    options.ProviderOptions.ClientId = "eventplatform-client";
    options.ProviderOptions.ResponseType = "id_token token";

    options.UserOptions.NameClaim = "email";
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.ScopeClaim = "scope";
});

builder.Services.AddBlazorBootstrap();

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);
builder.Services.AddHttpClient("Public", client => client.BaseAddress = baseAddress).AddHttpMessageHandler<HttpStatusCodeHandler>();
builder.Services.AddHttpClient("Authorized", client => client.BaseAddress = baseAddress)
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>().AddHttpMessageHandler<HttpStatusCodeHandler>();

builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(KeyCloakAccountFactory));

builder.Services.AddScoped<IIdentityService, IdentityService>();
builder.Services.AddScoped<IMessageService, MessageService>();
builder.Services.AddTransient<IEventService, EventService.Client.Services.EventService>();
builder.Services.AddTransient<HttpStatusCodeHandler>();

await builder.Build().RunAsync();
