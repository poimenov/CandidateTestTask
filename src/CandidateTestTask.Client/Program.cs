using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CandidateTestTask.Client;
using CandidateTestTask.Client.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["BackendUrl"] ?? builder.HostEnvironment.BaseAddress) });
builder.Services.AddTransient<ICandidateService, CandidateService>();

await builder.Build().RunAsync();
