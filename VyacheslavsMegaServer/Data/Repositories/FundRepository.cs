using Microsoft.EntityFrameworkCore;
using System.Transactions;
using VyacheslavsMegaServer.Data.Entities;

namespace VyacheslavsMegaServer.Data.Repositories
{
    public class FundRepository : Base.RepositoryBase
    {
        public enum TransactionState { Added, Removed }

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

        public async Task<List<FundTransaction>> GetTransactionsByDate(DateTime dateStart, DateTime dateEnd)
        {
            return await DB.FundTransactions.Where(t => t.Date >= dateStart && t.Date <= dateEnd).ToListAsync();
        }

        public async Task<FundTransaction> GetTransactionById(int id)
        {
            return await DB.FundTransactions.FirstAsync(t => t.Id == id);
        }

        public async Task SaveTransaction(FundTransaction transaction)
        {
            using (var DBTransaction = await DB.Database.BeginTransactionAsync())
            {
                try
                {
                    if (transaction.Id == 0)
                    {
                        DB.Entry(transaction).State = EntityState.Added;
                        await recalcSnapshotsByTransaction(transaction, TransactionState.Added);
                    }
                    else
                    {
                        FundTransaction oldTransaction = await GetTransactionById(transaction.Id);
                        if (transaction.Amount == oldTransaction.Amount && transaction.Date == oldTransaction.Date) 
                        {
                            DB.Entry(oldTransaction).State = EntityState.Detached;
                            DB.Entry(transaction).State = EntityState.Modified;
                        }
                        else
                        {
                            await RemoveTransactionById(transaction.Id);
                            await recalcSnapshotsByTransaction(oldTransaction, TransactionState.Removed);
                            DB.FundTransactions.Add(transaction);
                            await recalcSnapshotsByTransaction(transaction, TransactionState.Added);
                        }
                    }
                        
                    await DB.SaveChangesAsync();
                    await DBTransaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    DBTransaction.Rollback();
                }
                return;
            }
        }

        public async Task RemoveTransactionById(int id)
        {
            FundTransaction transaction = await GetTransactionById(id);
            DB.FundTransactions.Remove(transaction);
            await DB.SaveChangesAsync();
        }

        private IQueryable<FundSnapshot> getFundSnapshotsAfterDate(DateTime date)
        {
            return DB.FundSnapshots.Where(s => s.SnapshotDate >= date);
        }

        private async Task recalcSnapshotsByTransaction(FundTransaction transaction, TransactionState transactionState)
        {
            foreach (FundSnapshot snapshot in getFundSnapshotsAfterDate(transaction.Date))
            {
                snapshot.Balance += transactionState == TransactionState.Added ? transaction.Amount : -transaction.Amount; 
            }
            await DB.SaveChangesAsync();
            return;
        }
    }
}
