using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIRE.X.DL
{
    sealed class Context : DbContext
    {
        //Data Source=D:\\FIRE.sdf;Encrypt Database=True;Password=test;
        public Context(): base("Data Source=D:\\FIRE.sdf;Encrypt Database=False;")
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
    }

    public static class ContextHelpers
    {
        public static List<Transaction> GetTransactions()
        {
            using (var db = new Context())
            {
                return db.Transactions.ToList();
            }
        }

        public static List<Transaction> GetTransactions(TransactionSource transactionSource, DateTime from, DateTime to)
        {
            using (var db = new Context())
            {
                return db.Transactions
                    .Where(d => d.Source == transactionSource)
                    .Where(d => d.Date >= from && d.Date <= to).ToList();
            }
        }

        public static Dictionary<TransactionSource, DateTime?[]> GetRange()
        {
            using (var db = new Context())
            {
                var minMax = db.Transactions
                    .OrderBy(f => f.Date)
                    .Where(f => f.Date.HasValue)
                    .GroupBy(f => f.Source)
                    .ToDictionary(f => f.Key, c => new DateTime?[] { c.Min(f => f.Date), c.Max(f => f.Date) });

                return minMax;
            }
        }

        public static async Task<int> AddTransactions(params Transaction[] transactions)
        {
            using(var db = new Context())
            {
                var _transactions = GetTransactions();

                foreach(var transaction in transactions)
                {
                    if (!_transactions.Any(f => f.Source == transaction.Source && f.TransactionId == transaction.TransactionId))
                    {
                        db.Transactions.Add(transaction);
                    }
                }
                return await db.SaveChangesAsync();
            }
        }

        public static TransactionSource[] TransactionSources()
        {
            using(var db = new Context())
            {
                return db.Transactions.Select(f => f.Source).Distinct().ToArray();
            }
        }
    }
}
