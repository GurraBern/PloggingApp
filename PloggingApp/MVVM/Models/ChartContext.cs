using CommunityToolkit.Mvvm.ComponentModel;
using Microcharts;

namespace PloggingApp.MVVM.Models;
public class ChartContext : ObservableObject
{

    private Chart chart;
    public Chart Chart
    {
        get => chart;
        set => SetProperty(ref chart, value);
    }
    public string Name { get; set; }
    public string Unit { get; set; }
    
}
