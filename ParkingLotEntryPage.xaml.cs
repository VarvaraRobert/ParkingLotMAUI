using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class ParkingLotEntryPage : ContentPage
{
    public ParkingLotEntryPage()
    {
        InitializeComponent();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        listView.ItemsSource = await App.Database.GetParkingLotsAsync();
    }

    async void OnAddButtonClicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ParkingLotPage
        {
            BindingContext = new ParkingLot()
        });
    }

    async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null) return;

        var lot = (ParkingLot)e.SelectedItem;
        await Navigation.PushAsync(new ParkingLotPage
        {
            BindingContext = lot
        });

        listView.SelectedItem = null;
    }
}