using SQLite;

namespace ParkingLotMAUI.Models
{
    public class PlanParkingLot
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int SubscriptionPlanID { get; set; }
        public int ParkingLotID { get; set; }
    }
}
