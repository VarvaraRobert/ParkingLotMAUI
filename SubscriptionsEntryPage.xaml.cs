using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class SubscriptionsEntryPage : ContentPage
{
    public SubscriptionsEntryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        var subs = await App.Database.GetSubscriptionsAsync();
        var plans = await App.Database.GetSubscriptionPlansAsync();

        var items = subs.Select(s =>
        {
            var planName = plans.FirstOrDefault(p => p.ID == s.SubscriptionPlanID)?.Name ?? "Unknown plan";
            return new
            {
                Subscription = s,
                Title = $"{s.SubscriberName} - {planName}",
                Detail = $"Ends: {s.EndDate:yyyy-MM-dd}"
            };
        }).ToList();

        listView.ItemsSource = items;
    }

    async void OnAddClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SubscriptionCreatePage());
    }

    async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        if (sender is ListView lv)
            lv.SelectedItem = null;

        dynamic item = e.SelectedItem;
        Subscription sub = item?.Subscription as Subscription;

        if (sub == null)
        {
            await DisplayAlert("Error", "Selected subscription is null.", "OK");
            return;
        }

        // IMPORTANT: PushAsync -> Back arrow shows automatically
        await Navigation.PushAsync(new SubscriptionDetailsPage(sub.ID));
    }
}