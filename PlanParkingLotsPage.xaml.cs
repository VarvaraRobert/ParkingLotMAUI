using ParkingLotMAUI.Models;
using ParkingLotMAUI.ViewModels;

namespace ParkingLotMAUI;

public partial class PlanParkingLotsPage : ContentPage
{
    private readonly SubscriptionPlan _plan;

    public PlanParkingLotsPage(SubscriptionPlan plan)
    {
        InitializeComponent();
        _plan = plan;
        PlanNameLabel.Text = $"Plan: {plan.Name}";
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await LoadDataAsync();
    }

    private async Task LoadDataAsync()
    {
        var lots = await App.Database.GetParkingLotsAsync();
        var links = await App.Database.GetPlanParkingLotsAsync(_plan.ID);

        var assignedIds = new HashSet<int>(links.Select(x => x.ParkingLotID));

        var items = lots.Select(l => new ParkingLotSelectionItem
        {
            ParkingLotID = l.ID,
            Name = l.Name,
            City = l.City,
            IsAssigned = assignedIds.Contains(l.ID)
        }).ToList();

        listView.ItemsSource = items;
    }

    async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem is not ParkingLotSelectionItem item)
            return;

        await App.Database.ToggleParkingLotForPlanAsync(_plan.ID, item.ParkingLotID);

        listView.SelectedItem = null;
        await LoadDataAsync();
    }
}