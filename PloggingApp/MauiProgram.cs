using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.Pages;
using PloggingApp.Pages.Leaderboard;
using RestSharp;
using System.Reflection;

namespace PloggingApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            .UseMauiCommunityToolkitCore()
            .AddAppSettings()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        AddApiClients(builder);
        AddViewModels(builder);
        AddPages(builder);
        AddServices(builder);

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void AddViewModels(MauiAppBuilder builder)
    {
        //Pages ViewModels
        builder.Services.AddTransient<RankingViewmodel>();

        //Views ViewModels
        builder.Services.AddTransient<LeaderboardViewModel>();
    }

    private static void AddPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<RankingPage>();
    }

    private static void AddServices(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IRankingService, RankingService>();
    }

    private static void AddApiClients(MauiAppBuilder builder)
    {
        var apiUrl = builder.Configuration["ApiUrls:PloggingApiUrl"];
        if (apiUrl != null)
        {
            var ploggingApiClient = new RestClient(apiUrl);
            builder.RegisterPloggingApiClient<PloggingSession>(ploggingApiClient);
        }
    }

    private static void RegisterPloggingApiClient<T>(this MauiAppBuilder builder, IRestClient restClient)
    {
        builder.Services.AddTransient<IPloggingApiClient<T>>(serviceProvider =>
        {
            return new PloggingApiClient<T>(restClient);
        });
    }

    private static MauiAppBuilder AddAppSettings(this MauiAppBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("MAUI_ENVIRONMENT") ?? "Production";
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"PloggingApp.appsettings.{environment}.json");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

        if (stream != null)
        {
            IConfigurationRoot config = new ConfigurationBuilder()
                .AddJsonStream(stream)
                .Build();

            builder.Configuration.AddConfiguration(config);
        }

        return builder;
    }
}
