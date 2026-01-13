using SQLite;

namespace ParkingLotMAUI.Models
{
    public class Subscription
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; }

        public int SubscriptionPlanID { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string SubscriberName { get; set; } = string.Empty;
        public bool ReminderEnabled { get; set; }
        public DateTime? ReminderDate { get; set; }
    }
}