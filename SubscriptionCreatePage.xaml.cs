using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class SubscriptionCreatePage : ContentPage
{
    private List<SubscriptionPlan> _plans = new();

    public SubscriptionCreatePage()
    {
        InitializeComponent();
        startDatePicker.Date = DateTime.Today;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _plans = await App.Database.GetSubscriptionPlansAsync();
        planPicker.ItemsSource = _plans;
        planPicker.ItemDisplayBinding = new Binding("Name");

        if (_plans.Count > 0 && planPicker.SelectedIndex < 0)
            planPicker.SelectedIndex = 0;

        UpdateEndDate();
    }

    void OnChanged(object sender, EventArgs e) => UpdateEndDate();

    void UpdateEndDate()
    {
        if (planPicker.SelectedIndex < 0 || planPicker.SelectedIndex >= _plans.Count)
        {
            endDateLabel.Text = "End date: -";
            return;
        }

        var plan = _plans[planPicker.SelectedIndex];
        var end = startDatePicker.Date.AddDays(plan.DurationDays);
        endDateLabel.Text = $"End date: {end:yyyy-MM-dd}";
    }

    async void OnSaveClicked(object sender, EventArgs e)
    {
        if (planPicker.SelectedIndex < 0)
        {
            await DisplayAlert("Missing", "Select a plan.", "OK");
            return;
        }

        var name = subscriberEntry.Text?.Trim() ?? "";
        if (string.IsNullOrWhiteSpace(name))
        {
            await DisplayAlert("Missing", "Enter subscriber name.", "OK");
            return;
        }

        var plan = _plans[planPicker.SelectedIndex];
        var start = startDatePicker.Date;
        var end = start.AddDays(plan.DurationDays);

        var sub = new Subscription
        {
            SubscriberName = name,
            SubscriptionPlanID = plan.ID,
            StartDate = start,
            EndDate = end
        };

        await App.Database.SaveSubscriptionAsync(sub);

        await DisplayAlert("Saved", $"Subscription ends on {end:yyyy-MM-dd}.", "OK");
        await Navigation.PopAsync();
    }
}