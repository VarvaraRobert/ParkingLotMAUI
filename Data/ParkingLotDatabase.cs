using SQLite;
using ParkingLotMAUI.Models;

namespace ParkingLotMAUI.Data
{
    public class ParkingLotDatabase
    {
        private readonly SQLiteAsyncConnection _database;

        public ParkingLotDatabase(string dbPath)
        {
            _database = new SQLiteAsyncConnection(dbPath);

            _database.CreateTableAsync<ParkingLot>().Wait();
            _database.CreateTableAsync<SubscriptionPlan>().Wait();
            _database.CreateTableAsync<PlanParkingLot>().Wait();
        }

        // -------------------------
        // ParkingLots
        // -------------------------
        public Task<List<ParkingLot>> GetParkingLotsAsync()
            => _database.Table<ParkingLot>().ToListAsync();

        public Task<ParkingLot?> GetParkingLotAsync(int id)
            => _database.Table<ParkingLot>()
                        .Where(i => i.ID == id)
                        .FirstOrDefaultAsync();

        public Task<int> SaveParkingLotAsync(ParkingLot lot)
        {
            if (lot.ID != 0) return _database.UpdateAsync(lot);
            return _database.InsertAsync(lot);
        }

        public Task<int> DeleteParkingLotAsync(ParkingLot lot)
            => _database.DeleteAsync(lot);

        // -------------------------
        // SubscriptionPlans
        // -------------------------
        public Task<List<SubscriptionPlan>> GetSubscriptionPlansAsync()
            => _database.Table<SubscriptionPlan>().ToListAsync();

        public Task<int> SaveSubscriptionPlanAsync(SubscriptionPlan plan)
        {
            if (plan.ID != 0) return _database.UpdateAsync(plan);
            return _database.InsertAsync(plan);
        }

        public Task<int> DeleteSubscriptionPlanAsync(SubscriptionPlan plan)
            => _database.DeleteAsync(plan);

        // -------------------------
        // PlanParkingLot
        // -------------------------
        public Task<List<PlanParkingLot>> GetPlanParkingLotsAsync(int planId)
            => _database.Table<PlanParkingLot>()
                        .Where(x => x.SubscriptionPlanID == planId)
                        .ToListAsync();

        public Task<PlanParkingLot?> GetPlanParkingLotAsync(int planId, int lotId)
            => _database.Table<PlanParkingLot>()
                        .Where(x => x.SubscriptionPlanID == planId && x.ParkingLotID == lotId)
                        .FirstOrDefaultAsync();

        public Task<int> SavePlanParkingLotAsync(PlanParkingLot link)
        {
            if (link.ID != 0) return _database.UpdateAsync(link);
            return _database.InsertAsync(link);
        }

        public Task<int> DeletePlanParkingLotAsync(PlanParkingLot link)
            => _database.DeleteAsync(link);

        public async Task ToggleParkingLotForPlanAsync(int planId, int lotId)
        {
            var existing = await GetPlanParkingLotAsync(planId, lotId);

            if (existing != null)
            {
                await _database.DeleteAsync(existing);
            }
            else
            {
                await _database.InsertAsync(new PlanParkingLot
                {
                    SubscriptionPlanID = planId,
                    ParkingLotID = lotId
                });
            }
        }
    }
}