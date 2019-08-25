using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FIRE.X.DL
{
    sealed class Context : DbContext
    {
        //Data Source=D:\\FIRE.sdf;Encrypt Database=True;Password=test;
        public Context() : base("Data Source=D:\\FIRE.sdf;Encrypt Database=False;")
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Transaction>().Property(p => p.Amount).HasPrecision(16, 9);
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

        public static List<Transaction> GetTransactions(Expression<Func<Transaction, bool>> transactionSource, DateTime from, DateTime to)
        {
            using (var db = new Context())
            {
                return db.Transactions
                    .Where(transactionSource)
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
                    .ToDictionary(f => f.Key, c => new DateTime?[] { c.OrderBy(f => f.Date).First().Date, c.OrderByDescending(f => f.Date).First().Date });

                return minMax;
            }
        }

        public static async Task<int> AddTransactions(params Transaction[] transactions)
        {
            using (var db = new Context())
            {
                var _transactions = GetTransactions();

                foreach (var transaction in transactions)
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
            using (var db = new Context())
            {
                return db.Transactions.Select(f => f.Source).Distinct().ToArray();
            }
        }

        // other stats
        public static List<InvestmentPerSource> InvestmentsPerSource()
        {
            using (var db = new Context())
            {
                return db.Transactions.GroupBy(t => t.Source, (s, e) =>
                new InvestmentPerSource() {
                    Source = s,
                    Amount = e.Where(t => t.TransactionType == TransactionType.Deposit || t.TransactionType == TransactionType.Withdraw).Sum(t => t.Amount)
                }).ToList();
            }
        }

        public static List<InvestmentInterestPerMonthPerYearPerSource> InvestmentInterestPerMonthPerYearPerSource()
        {
            using (var db = new Context())
            {
                return db.Transactions
                    .Where(t => t.TransactionType == TransactionType.Interest)
                    .GroupBy(t => new
                    {
                        Source = t.Source,
                        Year = t.Date.Value.Year,
                        Month = t.Date.Value.Month
                    }).Select(v => new InvestmentInterestPerMonthPerYearPerSource
                    {
                        Year = v.Key.Year,
                        Month = v.Key.Month,
                        Amount = v.Sum(t => t.Amount),
                        Source = v.Key.Source
                    }).ToList();

            }
        }
    }

    public class InvestmentInterestPerMonthPerYearPerSource {
        public int Year;
        public int Month;
        public TransactionSource Source;
        public decimal Amount;

        public InvestmentInterestPerMonthPerYearPerSource()
        {

        }
    }

    public class InvestmentPerSource
    {
        public decimal Amount;
        public TransactionSource Source;
    }
}
