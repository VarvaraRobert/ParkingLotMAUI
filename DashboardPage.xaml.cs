using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class DashboardPage : ContentPage
{
    public DashboardPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var lots = await App.Database.GetParkingLotsAsync();
        var plans = await App.Database.GetSubscriptionPlansAsync();
        var subs = await App.Database.GetSubscriptionsAsync();

        var now = DateTime.Now.Date;
        var limit = now.AddDays(3);

        var expiring = subs
            .Where(s => s.EndDate.Date >= now && s.EndDate.Date <= limit)
            .OrderBy(s => s.EndDate)
            .ToList();

        var today = DateTime.Now.Date;

        var dueReminders = subs
            .Where(s => s.ReminderEnabled && s.ReminderDate.HasValue && s.ReminderDate.Value.Date <= today)
            .ToList();

        if (dueReminders.Any())
        {
            var first = dueReminders.First();
            var planName = plans.FirstOrDefault(p => p.ID == first.SubscriptionPlanID)?.Name ?? "Unknown plan";

            await DisplayAlert("Reminder",
                $"Subscription for {first.SubscriberName} ({planName}) ends on {first.EndDate:yyyy-MM-dd}.",
                "OK");
        }

        parkingLotsCountLabel.Text = lots.Count.ToString();
        plansCountLabel.Text = plans.Count.ToString();
        subsCountLabel.Text = subs.Count.ToString();
        expiringCountLabel.Text = expiring.Count.ToString();

        var items = expiring.Select(s =>
        {
            var planName = plans.FirstOrDefault(p => p.ID == s.SubscriptionPlanID)?.Name ?? "Unknown plan";
            return new
            {
                Title = $"{s.SubscriberName} - {planName}",
                Detail = $"Ends: {s.EndDate:yyyy-MM-dd}"
            };
        }).ToList();

        expiringList.ItemsSource = items;
    }
}