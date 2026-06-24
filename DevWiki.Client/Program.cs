using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DevWiki.Client;

using Microsoft.AspNetCore.Components.Authorization;
using DevWiki.Client.Auth;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// Регистрация сервисов авторизации
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<CookieHandler>();

// Настраиваем HttpClient с CookieHandler, чтобы он отправлял HttpOnly куки в кросс-доменных запросах
builder.Services.AddScoped(sp => 
{
    var handler = sp.GetRequiredService<CookieHandler>();
    handler.InnerHandler = new HttpClientHandler();
    return new HttpClient(handler) { BaseAddress = new Uri("http://localhost:5285") };
});

// Регистрируем наши сервисы
builder.Services.AddScoped<DevWiki.Client.Services.IAuthService, DevWiki.Client.Services.AuthService>();
builder.Services.AddScoped<DevWiki.Client.Services.IArticleService, DevWiki.Client.Services.ArticleService>();
builder.Services.AddSingleton<DevWiki.Client.Services.IToastService, DevWiki.Client.Services.ToastService>();

await builder.Build().RunAsync();
