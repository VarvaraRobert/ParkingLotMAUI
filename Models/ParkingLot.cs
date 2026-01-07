using SQLite;

namespace ParkingLotMAUI.Models
{
    public class ParkingLot
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;

        public int Capacity { get; set; }
        public decimal HourlyRate { get; set; }
    }
}