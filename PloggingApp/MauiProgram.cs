using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Maps;
using Microcharts.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Plogging.Core.Models;
using PloggingApp.Data.Services;
using PloggingApp.Data.Services.ApiClients;
using PloggingApp.Data.Services.Interfaces;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.MVVM.Views;
using PloggingApp.Pages;
using PloggingApp.Services.Camera;
using PloggingApp.Services.PloggingTracking;
using PloggingApp.Services.Authentication;
using RestSharp;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection;
using PloggingApp.Services.Statistics;
using PloggingApp.Services;
using ZXing.Net.Maui.Controls;

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
            .UseMicrocharts()
            .UseSkiaSharp()
            .UseMauiCommunityToolkitMaps("AoUR4E62oR7u3eyHLolc9rR0ofWn0p0DrczTs1d6oIQCwkUmla3SCdnzdftVvCMS") /*FÖR WINDOWS */
            .UseMauiMaps() /*android och IOS specific*/
            .UseBarcodeReader()

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        AddApiClients(builder);
        AddServices(builder);
        AddViewModels(builder);
        AddPopups(builder);
        AddPages(builder);

        builder.ConfigureMauiHandlers(handlers =>
        {
            handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
        });

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void AddViewModels(MauiAppBuilder builder)
    {
        //Pages ViewModels
        builder.Services.AddTransient<RankingViewmodel>();
        builder.Services.AddTransient<DashboardViewModel>();
        builder.Services.AddTransient<MapPageViewModel>();
        builder.Services.AddTransient<StatisticsPageViewModel>();

        builder.Services.AddScoped<CheckoutImageViewModel>();
        builder.Services.AddTransient<OthersProfilePageViewModel>();
        builder.Services.AddSingleton<AuthenticationViewModel>();

        builder.Services.AddTransient<SessionStatisticsViewModel>();

        //Views ViewModels
        builder.Services.AddTransient<LeaderboardViewModel>();
        builder.Services.AddTransient<StatisticsViewModel>();
        builder.Services.AddTransient<StreakViewModel>();
        builder.Services.AddTransient<OthersSessionsViewModel>();
        builder.Services.AddTransient<MapViewModel>();
        builder.Services.AddTransient<AddLitterViewModel>();
        builder.Services.AddTransient<PloggingSessionViewModel>();
        builder.Services.AddTransient<PlogTogetherViewModel>();
        builder.Services.AddTransient<GenerateQRcodeViewModel>();
        builder.Services.AddTransient<ScanQRcodePageViewModel>();
        builder.Services.AddTransient<MyProfileViewModel>();
    }

    private static void AddPopups(MauiAppBuilder builder)
    {
        builder.Services.AddTransientPopup<AcceptPopup, AcceptPopupViewModel>();
        builder.Services.AddTransientPopup<BadgesPopUpView,BadgesPopUpViewModel>();

        builder.Services.AddTransientPopup<LitterbagPlacementPopup, LitterbagPlacementViewModel>();
    }

    private static void AddPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<RankingPage>();
        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddTransient<MapPage>();

        builder.Services.AddTransient<DashboardPage>();

        builder.Services.AddTransient<StatisticsPage>();
        builder.Services.AddTransient<SessionStatisticsPage>();

        builder.Services.AddScoped<CheckoutImagePage>();
        builder.Services.AddScoped<GenerateQRcodePage>();
        builder.Services.AddTransient<ScanQRcodePage>();

        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

        builder.Services.AddTransient<OthersProfilePage>();
        builder.Services.AddTransient<PlogTogetherPage>();

        builder.Services.AddSingleton<MyProfilePage>();
    }

    private static void AddServices(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IToastService, ToastService>();
        builder.Services.AddTransient<IRankingService, RankingService>();
        builder.Services.AddTransient<IStreakService, StreakService>();
        builder.Services.AddTransient<IPloggingImageService, PloggingImageService>();
        builder.Services.AddScoped<ICameraService, CameraService>();
        builder.Services.AddSingleton<IPloggingSessionTracker, PloggingSessionTracker>();
        builder.Services.AddSingleton<IPloggingSessionService, PloggingSessionService>();
        builder.Services.AddSingleton<ILitterLocationService, LitterLocationService>();
        builder.Services.AddTransient<ILitterbagPlacementService, LitterbagPlacementService>();
        builder.Services.AddTransient<IChartService, ChartService>();
        builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();

        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = builder.Configuration["FirebaseApiKey"],
            AuthDomain = builder.Configuration["FirebaseUrl"],
            Providers = [new EmailProvider()]
        }));

        builder.Services.AddTransient<IPlogTogetherService, PlogTogetherService>();
        builder.Services.AddTransient<IUserInfoService, UserInfoService>();
    }

    private static void AddApiClients(MauiAppBuilder builder)
    {
        var apiUrl = builder.Configuration["ApiUrls:PloggingApiUrl"];
        if (apiUrl != null)
        {
            var ploggingApiClient = new RestClient(apiUrl);
            builder.RegisterPloggingApiClient<PloggingSession>(ploggingApiClient);
            builder.RegisterPloggingApiClient<UserStreak>(ploggingApiClient);
            builder.RegisterPloggingApiClient<LitterLocation>(ploggingApiClient);
            builder.RegisterPloggingApiClient<PlogTogether>(ploggingApiClient);
            builder.RegisterPloggingApiClient<Plogging.Core.Models.UserInfo>(ploggingApiClient);
            builder.RegisterPloggingApiClient<LitterbagPlacement>(ploggingApiClient);
            builder.RegisterPloggingApiClient<PloggingImage>(ploggingApiClient);
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
