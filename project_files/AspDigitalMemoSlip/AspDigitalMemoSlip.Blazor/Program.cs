using AspDigitalMemoSlip.Blazor;
using AspDigitalMemoSlip.Blazor.Providers;
using Blazored.LocalStorage;
using Blazored.Modal;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Add this line to get the ApiUrl from your appsettings.Development.json
var apiUrl = builder.Configuration.GetValue<string>("ApiUrl");

builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp =>
{
    var localStorage = sp.GetService<ILocalStorageService>();
    var authorizedHandler = new AuthorizedHandler(localStorage)
    {
        InnerHandler = new HttpClientHandler()
    };
    return new HttpClient(authorizedHandler) { BaseAddress = new Uri(apiUrl) }; // Use apiUrl here
});

// Add Blazored Modal
builder.Services.AddBlazoredModal();
// Add this line to register your ApplicationAuthenticationStateProvider
builder.Services.AddScoped<AuthenticationStateProvider, ApplicationAuthenticationStateProvider>();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
