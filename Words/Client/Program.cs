global using Microsoft.AspNetCore.Components.Authorization;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Words.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped(sp =>
{
    var handler = sp.GetRequiredService<CustormHttpHandler>();
    var httpClient = new HttpClient(handler) { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };
    return httpClient;
});


builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>(); // use our custom authentication state provider
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<CustormHttpHandler>();

await builder.Build().RunAsync();
