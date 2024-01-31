using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Core;
using Microsoft.Extensions.Logging;
using PloggingApp.Data.Context;
using PloggingApp.Data.Context.Interfaces;
using PloggingApp.MVVM.ViewModels;
using PloggingApp.MVVM.Views;

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
        builder.Services.AddTransient<RankingsViewModel>();
    }

    private static void AddPages(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<RankingsPage>();
    }

    private static void AddServices(MauiAppBuilder builder)
    {
        builder.Services.AddTransient<IRankingContext, RankingContext>();
    }
}
