using Microsoft.EntityFrameworkCore;
using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Data.Repositories
{
    public class FundGoalRepository : Base.RepositoryBase
    {
        public async Task<decimal> GetFundState()
        {
            FundSnapshot? lastSnapshot = DB.FundSnapshots.AsEnumerable().MaxBy(s => s.SnapshotDate);
            
            if (lastSnapshot is null)
                return await DB.FundTransactions.SumAsync(t => t.Amount);

            return lastSnapshot.Balance + await DB.FundTransactions.Where(t => t.Date > lastSnapshot.SnapshotDate).SumAsync(t => t.Amount);
        }

        public Goal? GetCurrentGoal()
        {
            return DB.Goals.AsEnumerable().MaxBy(g => g.Date);
        }

        public async Task<List<FundTransaction>> GetLastTransaction(int count)
        {
            return await DB.FundTransactions.OrderByDescending(t => t.Date).Take(count).ToListAsync();
        }
    }
}
