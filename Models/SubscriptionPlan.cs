using SQLite;

namespace ParkingLotMAUI.Models
{
    public class SubscriptionPlan
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public string Name { get; set; } = string.Empty;
        public decimal MonthlyPrice { get; set; }
        public int DurationDays { get; set; }
    }
}