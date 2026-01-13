using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class SubscriptionDetailsPage : ContentPage
{
    private readonly int _subscriptionId;
    private Subscription? _subscription;
    private SubscriptionPlan? _plan;
    private bool _ignoreReminderToggle;

    public SubscriptionDetailsPage(int subscriptionId)
    {
        InitializeComponent();
        _subscriptionId = subscriptionId;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        _subscription = await App.Database.GetSubscriptionAsync(_subscriptionId);
        if (_subscription == null)
        {
            await DisplayAlert("Not found", "Subscription not found.", "OK");
            await Navigation.PopAsync();
            return;
        }

        var plans = await App.Database.GetSubscriptionPlansAsync();
        _plan = plans.FirstOrDefault(p => p.ID == _subscription.SubscriptionPlanID);

        BindUI();
    }

    void BindUI()
    {
        if (_subscription == null) return;

        var planName = _plan?.Name ?? "Unknown plan";

        titleLabel.Text = $"{_subscription.SubscriberName} - {planName}";
        planLabel.Text = planName;
        startLabel.Text = _subscription.StartDate.ToString("yyyy-MM-dd");
        endLabel.Text = _subscription.EndDate.ToString("yyyy-MM-dd");

        var today = DateTime.Now.Date;
        var end = _subscription.EndDate.Date;
        var daysLeft = (end - today).Days;

        daysLeftLabel.Text = daysLeft.ToString();

        if (daysLeft < 0)
            statusLabel.Text = "Expired";
        else if (daysLeft <= 3)
            statusLabel.Text = "Expiring soon";
        else
            statusLabel.Text = "Active";

        extendButton.Text = _plan != null
            ? $"Extend {_plan.DurationDays} days"
            : "Extend 30 days";

        _ignoreReminderToggle = true;

        reminderSwitch.IsToggled = _subscription.ReminderEnabled;
        reminderStateLabel.Text = _subscription.ReminderEnabled ? "ON" : "OFF";

        if (_subscription.ReminderEnabled)
        {
            var reminderDate = _subscription.EndDate.Date.AddDays(-1);
            _subscription.ReminderDate = reminderDate;
            reminderInfoLabel.Text = $"Reminder scheduled for: {reminderDate:yyyy-MM-dd}";
        }
        else
        {
            reminderInfoLabel.Text = "Reminder is OFF";
        }

        _ignoreReminderToggle = false;
    }

    async void OnExtendClicked(object sender, EventArgs e)
    {
        if (_subscription == null) return;

        int daysToAdd = _plan?.DurationDays ?? 30;

        var baseDate = _subscription.EndDate.Date >= DateTime.Now.Date
            ? _subscription.EndDate.Date
            : DateTime.Now.Date;

        _subscription.EndDate = baseDate.AddDays(daysToAdd);

        if (_subscription.ReminderEnabled)
            _subscription.ReminderDate = _subscription.EndDate.Date.AddDays(-1);

        await App.Database.SaveSubscriptionAsync(_subscription);

        await DisplayAlert("Extended", $"New end date: {_subscription.EndDate:yyyy-MM-dd}", "OK");
        BindUI();
    }

    async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (_subscription == null) return;

        bool confirm = await DisplayAlert("Delete", "Delete this subscription?", "Yes", "No");
        if (!confirm) return;

        await App.Database.DeleteSubscriptionAsync(_subscription);
        await Navigation.PopAsync();
    }

    async void OnReminderToggled(object sender, ToggledEventArgs e)
    {
        if (_ignoreReminderToggle) return;
        if (_subscription == null) return;

        _subscription.ReminderEnabled = e.Value;

        if (_subscription.ReminderEnabled)
        {
            _subscription.ReminderDate = _subscription.EndDate.Date.AddDays(-1);
            reminderStateLabel.Text = "ON";
            reminderInfoLabel.Text = $"Reminder scheduled for: {_subscription.ReminderDate:yyyy-MM-dd}";
        }
        else
        {
            _subscription.ReminderDate = null;
            reminderStateLabel.Text = "OFF";
            reminderInfoLabel.Text = "Reminder is OFF";
        }

        await App.Database.SaveSubscriptionAsync(_subscription);
    }

    async void OnBackClicked(object sender, EventArgs e)
    {
        await Navigation.PopAsync();
    }
}