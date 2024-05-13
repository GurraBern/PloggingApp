namespace PloggingApp.Shared;

public interface IAsyncInitialization
{
    Task Initialization { get; }
}
