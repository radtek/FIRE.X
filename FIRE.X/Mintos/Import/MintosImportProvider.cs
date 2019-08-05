using CsvHelper;
using FIRE.X.DL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace FIRE.X.Mintos.Import
{
    public class MintosImportProvider : IImportProvider
    {
        public string GetName() => "Mintos";

        public TransactionSource GetTransactionSource() => TransactionSource.Mintos;

        private IProgress<int> Progress;

        public async Task<ImportResult<T>> GetRecords<T>(Stream file, Action<ImportResult<T>> done, Action<int> progress) where T : IImportModel
        {
            // initialize a progress
            Progress = new Progress<int>(progress);

            // start the task
            return await Task.Run(() =>
            {
                using (var reader = new StreamReader(file))
                using (var csvReader = new CsvReader(reader))
                {
                    return csvReader.GetRecords<T>().ToList();
                }
            }).ContinueWith(Collector).ContinueWith((t) =>
            {
                // when done, inform
                done(t.Result);

                return t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private ImportResult<IImportModel> Collector<IImportModel>(Task<List<IImportModel>> task)
        {
            var data = task.Result.Cast<MintosImport>().Where(d => d.Date.HasValue).OrderBy(d => d.Date);
            var interestRecords = data.Where(d => String.IsNullOrEmpty(d.Details) || d.Details.StartsWith("Interest income"));
            var investmentRecords = data.Where(d => !String.IsNullOrEmpty(d.Details) && d.Details.StartsWith("Investment principal increase Loan ID:"));
            var investmentPrincipals = data.Where(d => !String.IsNullOrEmpty(d.Details) && d.Details.StartsWith("Investment principal repayment Loan ID:"));

            var start = data.First().Date.Value.Date;
            var end = data.Last().Date.Value.Date;
            var dates = Enumerable.Range(0, 1 + end.Subtract(start).Days)
                              .Select(offset => start.AddDays(offset))
                              .ToArray();

           List<DateAmountSum> obj = new List<DateAmountSum>();

            for (int i = 0; i < dates.Count(); i++)
            {
                var interestRecordsOnThisDate = interestRecords.Where(x => x.Date?.Date == dates.ElementAt(i));
                var investmentRecordsOnThisDate = investmentRecords.Where(x => x.Date?.Date == dates.ElementAt(i));
                var investmentPrincipalsRecordsOnThisDate = investmentPrincipals.Where(x => x.Date?.Date == dates.ElementAt(i));

                if (!interestRecordsOnThisDate.Any())
                    continue;

                var r = new {
                    amount = interestRecordsOnThisDate.Sum(f => f.Turnover),
                    balance = interestRecordsOnThisDate.Last().Balance,
                    investment = investmentRecordsOnThisDate.Sum(f => f.Turnover),
                    principal = investmentPrincipalsRecordsOnThisDate.Sum(f => f.Turnover)
                };

                var ob = new DateAmountSum()
                {
                    Date = dates.ElementAt(i),
                    Amount = r.amount,
                    Sum = r.amount + (i == 0 ? 0 : (obj.Any() ? obj.Last().Sum : 0)),
                    Balance = r.balance,
                    Investment = r.investment,
                    Principal = r.principal
                };

                obj.Add(ob);

                Progress.Report((int)((decimal)i / dates.Count() * 100.0m));
            }

            return new ImportResult<IImportModel>()
            {
                ChartData = obj,
                ImportRules = task.Result
            };
        }
    }
}
