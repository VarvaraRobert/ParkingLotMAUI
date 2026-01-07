using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class SubscriptionPlanEntryPage : ContentPage
{
    public SubscriptionPlanEntryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetSubscriptionPlansAsync();
    }

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new SubscriptionPlanPage
        {
            BindingContext = new SubscriptionPlan()
        });
    }

    async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        var plan = (SubscriptionPlan)e.SelectedItem;
        await Navigation.PushAsync(new SubscriptionPlanPage
        {
            BindingContext = plan
        });

        listView.SelectedItem = null;
    }
}