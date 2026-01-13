using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class SubscriptionPlanPage : ContentPage
{
    public SubscriptionPlanPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var plan = (SubscriptionPlan)BindingContext;

        // simple validation (optional)
        if (string.IsNullOrWhiteSpace(plan.Name))
        {
            await DisplayAlert("Missing", "Please enter a name.", "OK");
            return;
        }

        await App.Database.SaveSubscriptionPlanAsync(plan);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var plan = (SubscriptionPlan)BindingContext;

        if (plan.ID == 0)
        {
            await Navigation.PopAsync();
            return;
        }

        await App.Database.DeleteSubscriptionPlanAsync(plan);
        await Navigation.PopAsync();
    }

    async void OnManageParkingLotsClicked(object sender, EventArgs e)
    {
        var plan = (SubscriptionPlan)BindingContext;

        // IMPORTANT: you must save first so it has ID
        if (plan.ID == 0)
        {
            await DisplayAlert("Save required", "Please save the plan before assigning parking lots.", "OK");
            return;
        }

        await Navigation.PushAsync(new PlanParkingLotsPage(plan));
    }
}