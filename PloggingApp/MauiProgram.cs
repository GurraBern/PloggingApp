﻿using CommunityToolkit.Maui;
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
using PloggingApp.Features.Authentication;
using PlogPal.Maui.Features.Authentication;
using PlogPal.Maui.Shared;
using PlogPal.Maui.Features.Dashboard;
using PloggingApp.Features.Dashboard;

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

        AddApiClients(builder);
        AddServices(builder);
        AddViewModels(builder);
        AddPopups(builder);
        AddPages(builder);



        SetupHandlers(builder.Services);

        //builder.ConfigureMauiHandlers(handlers =>
        //{
        //    handlers.AddHandler<Microsoft.Maui.Controls.Maps.Map, CustomMapHandler>();
        //});

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }

    private static void SetupHandlers(IServiceCollection services)
    {
        services.AddSingleton<IEventBus, EventBus>();

        var bus = services.GetRequiredService<IEventBus>();

        bus.Register<SignInEvent>(new SignInHandler());
    }

    private static void AddViewModels(MauiAppBuilder builder)
    {
        //Pages ViewModels
        builder.Services.AddTransient<LoginViewModel>();
        //builder.Services.AddTransient<RegisterViewModel>();

        builder.Services.AddTransient<DashboardViewModel>();
        //builder.Services.AddTransient<DashboardViewModel>();
        //builder.Services.AddTransient<StatisticsPageViewModel>();

        //builder.Services.AddScoped<CheckoutImageViewModel>();
        //builder.Services.AddTransient<OthersProfilePageViewModel>();
        //builder.Services.AddSingleton<RegisterViewModel>();

        //builder.Services.AddTransient<SessionStatisticsViewModel>();

        //builder.Services.AddTransient<HistoryPageViewModel>();
        //builder.Services.AddTransient<MyProfilePageViewModel>();

        //Views ViewModels
        //builder.Services.AddTransient<LeaderboardViewModel>();
        //builder.Services.AddTransient<StatisticsViewModel>();
        //builder.Services.AddTransient<StreakViewModel>();
        //builder.Services.AddTransient<OthersSessionsViewModel>();
        //builder.Services.AddTransient<MapViewModel>();
        //builder.Services.AddTransient<AddLitterViewModel>();
        //builder.Services.AddTransient<PloggingSessionViewModel>();
        //builder.Services.AddTransient<PlogTogetherViewModel>();
        //builder.Services.AddTransient<GenerateQRcodeViewModel>();
        //builder.Services.AddTransient<ScanQRcodePageViewModel>();
        //builder.Services.AddTransient<BadgesViewModel>();
        //builder.Services.AddTransient<SessionStatsMapViewModel>();
        //builder.Services.AddTransient<HistoryViewModel>();
        //builder.Services.AddTransient<ProfileInfoViewModel>();
    }

    private static void AddPopups(MauiAppBuilder builder)
    {
        //builder.Services.AddTransientPopup<BadgesPopUpView,BadgesPopUpViewModel>();
        //builder.Services.AddTransientPopup<LitterbagPlacementPopup, LitterbagPlacementViewModel>();
    }

    private static void AddPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<LoginPage>();
        builder.Services.AddTransient<RegisterPage>();

        builder.Services.AddTransient<DashboardPage>();

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
        //builder.Services.AddTransient<IPloggingSessionManager, PloggingSessionManager>();
        //builder.Services.AddTransient<ILocationProvider, MauiLocationProvider>();

        builder.Services.AddTransient<IToastService, ToastService>();
        ////builder.Services.AddSingleton<IRankingService, RankingService>();
        ////builder.Services.AddTransient<IStreakService, StreakService>();
        ////builder.Services.AddTransient<IPloggingImageService, PloggingImageService>();
        //builder.Services.AddScoped<ICameraService, CameraService>();
        //builder.Services.AddSingleton<IPloggingSessionTracker, PloggingSessionTracker>();
        //builder.Services.AddSingleton<IPloggingSessionService, PloggingSessionService>();
        //builder.Services.AddSingleton<ILitterLocationService, LitterLocationService>();
        //builder.Services.AddTransient<ILitterbagPlacementService, LitterbagPlacementService>();
        //builder.Services.AddTransient<IChartService, ChartService>();
        //builder.Services.AddSingleton<IAuthenticationService, AuthenticationService>();
        //builder.Services.AddTransient<ISessionStatisticsService, SessionStatisticsService>();


        builder.Services.AddSingleton<IAuthenticationService, FirebaseAuthentication>();
        builder.Services.AddSingleton<IUserContext, FirebaseUserContext>();
        builder.Services.AddSingleton(new FirebaseAuthClient(new FirebaseAuthConfig()
        {
            ApiKey = builder.Configuration["FirebaseApiKey"],
            AuthDomain = builder.Configuration["FirebaseUrl"],
            Providers = [new EmailProvider()]
        }));

        //builder.Services.AddTransient<IPlogTogetherService, PlogTogetherService>();
        //builder.Services.AddTransient<IUserInfoService, UserInfoService>();
    }

    private static void AddApiClients(MauiAppBuilder builder)
    {
        var apiUrl = builder.Configuration["ApiUrls:PloggingApiUrl"];
        if (apiUrl != null)
        {
            //var ploggingApiClient = new RestClient(apiUrl);
            //builder.RegisterPloggingApiClient<PlogSession>(ploggingApiClient);
            //builder.RegisterPloggingApiClient<UserStreak>(ploggingApiClient);
            //builder.RegisterPloggingApiClient<LitterLocation>(ploggingApiClient);
            //builder.RegisterPloggingApiClient<PlogTogether>(ploggingApiClient);
            //builder.RegisterPloggingApiClient<PlogPal.Domain.Models.UserInfo>(ploggingApiClient);
            //builder.RegisterPloggingApiClient<LitterbagPlacement>(ploggingApiClient);
            //builder.RegisterPloggingApiClient<PloggingImage>(ploggingApiClient);
        }
    }

    //private static void RegisterPloggingApiClient<T>(this MauiAppBuilder builder, IRestClient restClient)
    //{
    //    builder.Services.AddScoped<IPloggingApiClient<T>>(serviceProvider =>
    //    {
    //        var authService = serviceProvider.GetService<IAuthenticationService>();
    //        return new PloggingApiClient<T>(restClient, authService);
    //    });
    //}

    private static MauiAppBuilder AddAppSettings(this MauiAppBuilder builder)
    {
        var environment = Environment.GetEnvironmentVariable("MAUI_ENVIRONMENT") ?? "Production";
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
