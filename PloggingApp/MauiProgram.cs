using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using CommunityToolkit.Maui.Maps;
using Microcharts.Maui;
using Firebase.Auth;
using Firebase.Auth.Providers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection;
using ZXing.Net.Maui.Controls;
using PlogPal.Application.Common.Interfaces;
using Infrastructure.Authentication;
using PlogPal.Maui.Features.Authentication;
using PlogPal.Maui.Shared;
using PlogPal.Maui.Features.Dashboard;
using PloggingApp.Features.Dashboard;
using Infrastructure.EventBus;
using PlogPal.Domain.Events;
using PlogPal.Application.EventHandlers;
using Infrastructure.Services;
using Infrastructure.Services.ApiClients;
using RestSharp;
using PlogPal.Domain.Models;
using PlogPal.Application.LoginManagement.Commands;
using PloggingApp.Features.Map;
using PlogPal.Application;

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
            //.UseMauiMaps() /*android och IOS specific*/
            .UseBarcodeReader()

            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("Inter-SemiBold.ttf", "InterSemiBold");
                fonts.AddFont("Inter-Bold.ttf", "InterBold");
            });


        builder.Services.AddEventBus(Assembly.GetAssembly(typeof(SignInEvent)), Assembly.GetAssembly(typeof(SignInHandler)));

        AddApiClients(builder);
        AddServices(builder);
        AddCommands(builder);
        AddPopups(builder);
        AddPages(builder);

        //builder.ConfigureMauiHandlers(handlers =>
        //{
        //    handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
        //});

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void AddCommands(MauiAppBuilder builder)
    {
        builder.Services.AddScoped<ILoginCommand, LoginCommand>();
        builder.Services.AddScoped<IRegisterCommand, RegisterCommand>();
    }

    private static void AddPopups(MauiAppBuilder builder)
    {
        //builder.Services.AddTransientPopup<BadgesPopUpView,BadgesPopUpViewModel>();
        //builder.Services.AddTransientPopup<LitterbagPlacementPopup, LitterbagPlacementViewModel>();
    }

    private static void AddPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<LoginViewModel>();

        builder.Services.AddTransient<RegisterPage>();
        builder.Services.AddTransient<RegisterViewModel>();

        builder.Services.AddTransient<DashboardPage>();
        builder.Services.AddTransient<DashboardViewModel>();

        builder.Services.AddTransient<MapViewModel>();


        //builder.Services.AddTransientView<LeaderboardPage, LeaderboardViewModel>();

        //builder.Services.AddTransient<StatisticsPage>();
        //builder.Services.AddTransient<SessionStatisticsPage>();

        //builder.Services.AddScoped<CheckoutImagePage>();
        //builder.Services.AddScoped<GenerateQRcodePage>();
        //builder.Services.AddTransient<ScanQRcodePage>();

        //builder.Services.AddTransient<OthersProfilePage>();
        //builder.Services.AddTransient<PlogTogetherPage>();
        //builder.Services.AddTransient<MyProfilePage>();
        //builder.Services.AddTransient<HistoryPage>();
    }

    private static void AddServices(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IToastService, ToastService>();
        builder.Services.AddScoped<ILitterLocationService, LitterLocationService>();
        builder.Services.AddScoped<IStreakService, StreakService>();
        builder.Services.AddSingleton<IAuthenticationService, FirebaseAuthentication>();
        builder.Services.AddSingleton<IUserContext, UserContext>();
        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = builder.Configuration["FirebaseApiKey"],
            AuthDomain = builder.Configuration["FirebaseUrl"],
            Providers = [new EmailProvider()]
        }));
    }

    private static void AddApiClients(MauiAppBuilder builder)
    {
        var apiUrl = builder.Configuration["ApiUrls:PloggingApiUrl"];
        if (apiUrl != null)
        {
            var ploggingApiClient = new RestClient(apiUrl);
            builder.RegisterPloggingApiClient<UserStreak>(ploggingApiClient);
            builder.RegisterPloggingApiClient<LitterLocation>(ploggingApiClient);
        }
    }

    //todo use one client instead
    //todo middleware to handle bearer token
    private static void RegisterPloggingApiClient<T>(this MauiAppBuilder builder, IRestClient restClient)
    {
        builder.Services.AddScoped<IPloggingApiClient<T>>(serviceProvider =>
        {
            var userContext = serviceProvider.GetService<IUserContext>();
            return new PloggingApiClient<T>(restClient, userContext);
        });
    }

    private static MauiAppBuilder AddAppSettings(this MauiAppBuilder builder)
    {
        //var environment = Environment.GetEnvironmentVariable("MAUI_ENVIRONMENT") ?? "Production";
        var environment = "Production";
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
        using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"PlogPal.appsettings.{environment}.json");
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
