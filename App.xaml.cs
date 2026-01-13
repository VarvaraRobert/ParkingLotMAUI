using ParkingLotMAUI.Data;

namespace ParkingLotMAUI;

public partial class App : Application
{
    private static ParkingLotDatabase? _database;

    public static ParkingLotDatabase Database
    {
        get
        {
            if (_database == null)
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "ParkingLotSystem.db3");
                _database = new ParkingLotDatabase(dbPath);
            }
            return _database;
        }
    }

    public App()
    {
        InitializeComponent();
        MainPage = new NavigationPage(new AppShell());
    }
}