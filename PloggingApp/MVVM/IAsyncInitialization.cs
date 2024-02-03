namespace PloggingApp.MVVM;

public interface IAsyncInitialization
{
    Task Initialization { get; }
}
