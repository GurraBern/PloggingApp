using PloggingApp.Pages;

namespace PloggingApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CheckoutImagePage), typeof(CheckoutImagePage));
            Routing.RegisterRoute(nameof(DashboardPage), typeof(DashboardPage));
            Routing.RegisterRoute(nameof(ScanQRcodePage), typeof(ScanQRcodePage));
            Routing.RegisterRoute(nameof(GenerateQRcodePage), typeof(GenerateQRcodePage));
            Routing.RegisterRoute(nameof(SessionStatisticsPage), typeof(SessionStatisticsPage));
            Routing.RegisterRoute(nameof(MyProfilePage), typeof(MyProfilePage));
            Routing.RegisterRoute(nameof(OthersProfilePage), typeof(OthersProfilePage));
            Routing.RegisterRoute(nameof(DashboardPage) + "/" + nameof(PlogTogetherPage), typeof(PlogTogetherPage));
            Routing.RegisterRoute(nameof(HistoryPage), typeof(HistoryPage));

            Routing.RegisterRoute(nameof(UserEventsPage) + "/" + nameof(CreateEventPage), typeof(CreateEventPage));
            Routing.RegisterRoute(nameof(UserEventsPage) + "/" + nameof(CreateEventPage) + "/" + nameof(CreateEventDetails), typeof(CreateEventDetails));
        }
    }
}
