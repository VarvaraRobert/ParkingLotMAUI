using ParkingLotMAUI.Models;

namespace ParkingLotMAUI;

public partial class ParkingLotPage : ContentPage
{
    public ParkingLotPage()
    {
        InitializeComponent();
    }

    async void OnSaveButtonClicked(object sender, EventArgs e)
    {
        var lot = (ParkingLot)BindingContext;
        await App.Database.SaveParkingLotAsync(lot);
        await Navigation.PopAsync();
    }

    async void OnDeleteButtonClicked(object sender, EventArgs e)
    {
        var lot = (ParkingLot)BindingContext;
        await App.Database.DeleteParkingLotAsync(lot);
        await Navigation.PopAsync();
    }
}