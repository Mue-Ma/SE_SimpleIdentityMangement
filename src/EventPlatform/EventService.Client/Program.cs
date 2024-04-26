using EventPlatform.Common.Core.Factories;
using EventService.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddOidcAuthentication(options =>
{
#if DEBUG
    options.ProviderOptions.MetadataUrl = "http://localhost:8080/realms/EventPlatform/.well-known/openid-configuration";
    options.ProviderOptions.Authority = "http://localhost:8080/realms/EventPlatform";
#else
    options.ProviderOptions.MetadataUrl = "http://localhost/realms/EventPlatform/.well-known/openid-configuration";
    options.ProviderOptions.Authority = "http://localhost/realms/EventPlatform";
#endif
    options.ProviderOptions.ClientId = "eventplatform-client";
    options.ProviderOptions.ResponseType = "id_token token";

    options.UserOptions.NameClaim = "email";
    options.UserOptions.RoleClaim = "roles";
    options.UserOptions.ScopeClaim = "scope";
});

var baseAddress = new Uri(builder.HostEnvironment.BaseAddress);

builder.Services.AddHttpClient("Default", client => client.BaseAddress = baseAddress)
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Default"));
builder.Services.AddScoped(typeof(AccountClaimsPrincipalFactory<RemoteUserAccount>), typeof(KeyCloakAccountFactory));

builder.Services.AddBlazorBootstrap();

await builder.Build().RunAsync();
