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
        await App.Database.SaveSubscriptionPlanAsync(plan);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var plan = (SubscriptionPlan)BindingContext;
        await App.Database.DeleteSubscriptionPlanAsync(plan);
        await Navigation.PopAsync();
    }

    async void OnManageParkingLotsClicked(object sender, EventArgs e)
    {
        var plan = (SubscriptionPlan)BindingContext;

        if (plan.ID == 0)
        {
            await DisplayAlert("Save required", "Please save the plan before assigning parking lots.", "OK");
            return;
        }

        await Navigation.PushAsync(new PlanParkingLotsPage(plan));
    }
}