namespace ParkingLotMAUI.ViewModels
{
    public class ParkingLotSelectionItem
    {
        public int ParkingLotID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public bool IsAssigned { get; set; }
    }
}