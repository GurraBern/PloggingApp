using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;
using PloggingApp.MVVM.ViewModels;
using SkiaSharp;
namespace PloggingApp.Pages;
public partial class StatisticsPageViewModel : ObservableObject 
{
    // Hardcoded some entries to test
    ChartEntry[] trashEntries =
    [
        new ChartEntry(1)
     {
         Label = "Aluminium",
         ValueLabel = "27",
         Color = SKColor.Parse("#ffd86e")
     },
     new ChartEntry(2)
     {
         Label = "Plastic",
         ValueLabel = "55",
         Color = SKColor.Parse("#7aff6e")
     },
     new ChartEntry(3)
     {
         Label = "Other",
         ValueLabel = "14",
         Color = SKColor.Parse("#6e84ff")
     },

 ];
    ChartEntry[] activityEntries =
    [
        new ChartEntry(1)
     {
         Label = "Vecka 4",
         ValueLabel = "30",
         Color = SKColor.Parse("#ffd86e")
     },
     new ChartEntry(2)
     {
         Label = "Vecka 5",
         ValueLabel = "55",
         Color = SKColor.Parse("#7aff6e")
     },
     new ChartEntry(3)
     {
         Label = "Vecka 6",
         ValueLabel = "14",
         Color = SKColor.Parse("#6e84ff")
     },

 ];
    public StatisticsPageViewModel()
    {
        TrashTypeChart = new DonutChart()
        {
            LabelTextSize = 40,
            Entries = trashEntries
        };
        ActivityChart = new LineChart()
        {
            LabelTextSize = 40,
            Entries = activityEntries
        };
    }

    [ObservableProperty]
    Chart trashTypeChart;
    [ObservableProperty]
    Chart activityChart;
}
