using Excel.FinancialFunctions;
using FIRE.X.DL;
using FIRE.X.Properties;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace FIRE.X.Charts
{
    public class Charts : IChart
    {
        public Expression<Func<Transaction, bool>> GetTransactionSource() => (t) => t.Source == TransactionSource.Grupeer || t.Source == TransactionSource.Mintos;

        private double? IRR(IEnumerable<decimal> values, IEnumerable<DateTime> dates)
        {
            try
            {
                return Financial.XIrr(values.Select(f => (double)f), dates, 0.1) * 100;
            }
            catch
            {
                return null;
            }
        }
        public async Task<object[]> GetSeries(DateTime from, DateTime to)
        {
            return await Task.Run(() =>
            {
                var dateRange = Enumerable.Range(0, 1 + to.Subtract(from).Days)
                                  .Select(offset => from.AddDays(offset))
                                  .ToArray();

                var data = ContextHelpers.GetTransactions(GetTransactionSource(), from, to);

                var rentPerDay = new LineSeries()
                {
                    Title = Resources.RENT_DAY + " all",
                    Smooth = true
                };

                var serieInterestTotal = new LineSeries()
                {
                    Title = Resources.RENT_TOTAL + " all"
                };

                var serieBalance = new LineSeries()
                {
                    Title = Resources.BALANCE + " all"
                };

                var serieInvestment = new LineSeries()
                {
                    Title = Resources.INVESTMENTS + " all"
                };

                var serieDeposits = new LineSeries()
                {
                    Title = Resources.INVESTMENTS + " all"
                };

                var serieYield = new LineSeries()
                {
                    Title = "Yield all",
                    Smooth = true
                };

                var serieXirr = new LineSeries()
                {
                    Title = "Xirr all"
                };

                var dataForIRR =
                    data.Where(f => f.TransactionType == TransactionType.Interest || f.TransactionType == TransactionType.Investment || f.TransactionType == TransactionType.Principal);

                foreach (var date in dateRange)
                {
                    var transToday = data.Where(tr => tr.Date.Value.Date == date.Date).OrderBy(d => d.Date.Value);
                    double timestamp = OxyPlot.Axes.DateTimeAxis.ToDouble(date.Date);

                    var transactionsInterestToday = transToday.Where(t => t.TransactionType == TransactionType.Interest).Sum(f => f.Amount);
                    rentPerDay.Points.Add(new OxyPlot.DataPoint(timestamp, Convert.ToDouble(transactionsInterestToday)));

                    var sumInterest = data.Where(tr => tr.Date.Value.Date <= date.Date && tr.TransactionType == TransactionType.Interest).Sum(tr => tr.Amount);
                    serieInterestTotal.Points.Add(new OxyPlot.DataPoint(timestamp, Convert.ToDouble(sumInterest)));

                    var transactionBalance = transToday.Any() ? transToday.Last().Balance.Value : data.Where(f => f.Balance.HasValue && f.Date.Value <= date.Date).Last().Balance;
                    if (transactionBalance.HasValue)
                        serieBalance.Points.Add(new OxyPlot.DataPoint(timestamp, Convert.ToDouble(transactionBalance)));

                    decimal investmentsBalance = transToday.Where(t => t.TransactionType == TransactionType.Investment).Sum(f => f.Amount);
                    serieInvestment.Points.Add(new OxyPlot.DataPoint(timestamp, Convert.ToDouble(investmentsBalance)));

                    decimal desposits = transToday.Where(t => t.TransactionType == TransactionType.Deposit).Sum(f => f.Amount);
                    serieDeposits.Points.Add(new OxyPlot.DataPoint(timestamp, Convert.ToDouble(desposits)));

                    // Yield
                    var deposits = data.Where(f => f.Date.Value.Date <= date.Date && f.TransactionType == TransactionType.Deposit).Sum(f => f.Amount);
                    var withdraws = data.Where(f => f.Date.Value.Date <= date.Date && f.TransactionType == TransactionType.Withdraw).Sum(f => f.Amount);
                    var rent = data.Where(f => f.Date.Value.Date <= date.Date && f.TransactionType == TransactionType.Interest).Sum(f => f.Amount);
                    var totalAsset = deposits - withdraws;

                    serieYield.Points.Add(new OxyPlot.DataPoint(timestamp, Convert.ToDouble(rent / totalAsset) * 100));

                    // xirr
                    var gr = dataForIRR.Where(f => f.Date.Value <= date.Date)
                    .GroupBy(f => f.LoanId, (v, y) =>
                    new
                    {
                        transactions = y,
                        done = y.Where(f => f.TransactionType != TransactionType.Investment).Sum(f => f.Amount) > (y.First(f => f.TransactionType == TransactionType.Investment).Amount * -1),
                        loanId = v,
                        xiir = y.Any(f => f.TransactionType == TransactionType.Investment) && y.Any(f => f.TransactionType == TransactionType.Interest || f.TransactionType == TransactionType.Principal) && y.Where(f => f.TransactionType != TransactionType.Investment).Sum(f => f.Amount) > (y.First(f => f.TransactionType == TransactionType.Investment).Amount * -1) ? IRR(y.Select(f => f.Amount), y.Select(f => f.Date.Value)) : null
                    }).Where(f => f.done && f.xiir.HasValue);


                    if (gr.Any())
                        serieXirr.Points.Add(new OxyPlot.DataPoint(timestamp, gr.Average(v => v.xiir.Value)));
                }

                return new object[7] {
                rentPerDay,
                serieInterestTotal,
                serieBalance,
                serieInvestment,
                serieDeposits,
                serieYield,
                serieXirr
                };
            });
        }

        public DateTime?[] MaxRange()
        {
            DateTime? lowest = DateTime.Now;
            DateTime? highest = DateTime.Now;

            foreach(var dt in ContextHelpers.GetRange())
            {
                var low = dt.Value[0].Value;
                if (low < lowest)
                    lowest = low;

                var high = dt.Value[1].Value;
                if (high > highest)
                    highest = high;
            }

            return new DateTime?[2] { lowest, highest };
        }
    }
}
