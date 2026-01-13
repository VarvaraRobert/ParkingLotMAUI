namespace ParkingLotMAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(SubscriptionDetailsPage), typeof(SubscriptionDetailsPage));
        }
    }
}
