using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using PloggingApp.Data.Context;
using PloggingApp.Data.Context.Interfaces;
using PloggingApp.Features.Leaderboard;
using PloggingApp.Pages;
using PloggingApp.Pages.Leaderboard;

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
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

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
        builder.Services.AddTransient<IRankingContext, RankingContext>();
    }
}
