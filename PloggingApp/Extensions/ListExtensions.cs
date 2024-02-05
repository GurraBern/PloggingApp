using System.Collections.ObjectModel;

namespace PloggingApp.Extensions;

public static class ListExtensions
{
    public static void AddRange<T>(this ObservableCollection<T> observableCollection, IEnumerable<T> list)
    {
        observableCollection.Clear();
        foreach (var nutrient in list)
        {
            observableCollection.Add(nutrient);
        }
    }
}
