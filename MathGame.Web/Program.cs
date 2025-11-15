using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MathGame.Web;
using MudBlazor.Services;
using MathGame.Web.Services;
using MathGame.Web.Models.Quiz;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var appBasePath = builder.Configuration.GetValue<string>("AppBasePath");

builder.Services.AddScoped<QuizService>();
builder.Services.AddScoped<SpotifyService>();
builder.Services.AddScoped<CsvQuizParser>();
builder.Services.AddScoped<QuizFactory>();
Console.WriteLine($"AppBasePath: {appBasePath}");

builder.Services.AddMudServices();
await builder.Build().RunAsync();
