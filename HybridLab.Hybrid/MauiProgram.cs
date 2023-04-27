using BlazorStrap;
using HybridLab.Core;
using HybridLab.Core.Clients;
using HybridLab.Data;
using HybridLab.Hybrid.Data;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace HybridLab.Hybrid
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            var assembly = Assembly.GetExecutingAssembly();
            using var stream = assembly.GetManifestResourceStream("HybridLab.Hybrid.appsettings.json");

            var configuration = new ConfigurationBuilder()
                        .AddJsonStream(stream)
            .Build();

            builder.Configuration.AddConfiguration(configuration);
            builder.Services.AddSingleton<IAppKeysAccess, AppKeysAccess>();
            builder.Services.AddSingleton<IAppKeys, AppDataAppKeys>();
            builder.Services.AddBlazorStrap();
            builder.Services.AddHttpClient<MainHttpClient>(httpClient => httpClient.BaseAddress = new Uri(builder.Configuration["ServiceUrl"]));
            builder.Services.AddAuthorizationCore();
            builder.Services.AddScoped<AuthenticationStateProvider, MainAuthenticationStateProvider>();
            builder.Services.AddSingleton<IDataBaseAccess, DataBaseAccess>();
            builder.Services.AddSingleton<IAppData, AppData>();
            builder.Services.AddSingleton<IAccountClient, AccountClient>();

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<WeatherForecastService>();

            return builder.Build();
        }
    }
}