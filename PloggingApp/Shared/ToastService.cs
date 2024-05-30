using CommunityToolkit.Maui.Alerts;
using CommunityToolkit.Maui.Core;

namespace PlogPal.Maui.Shared;

public class ToastService : IToastService
{
    private readonly CancellationTokenSource cancellationTokenSource = new();

    public async Task MakeToast(string message)
    {
        var toast = Toast.Make(message, ToastDuration.Short);
        await toast.Show(cancellationTokenSource.Token);
    }

    public async Task MakeToast(string message, ToastDuration duration)
    {
        var toast = Toast.Make(message, duration);
        await toast.Show(cancellationTokenSource.Token);
    }
}

public interface IToastService
{
    Task MakeToast(string message);

    Task MakeToast(string message, ToastDuration duration);
}
