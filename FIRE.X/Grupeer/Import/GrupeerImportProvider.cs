using CsvHelper;
using FIRE.X.DL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIRE.X.Grupeer.Import
{
    public class GrupeerImportProvider<T> : ImportProvider where T : GrupeerImport
    {
        public override string GetName() => "Grupeer";

        public override TransactionSource GetTransactionSource() => TransactionSource.Mintos;

        private ImportResult<IImportModel> Collector(Task<List<GrupeerImport>> task)
        {
            return new ImportResult<IImportModel>()
            {
                RealType = task.Result.GetType().GetGenericArguments()[0],
                ImportRules = task.Result.ToList<IImportModel>()
            };
        }

        private IProgress<int> Progress;
        public override async Task<ImportResult<IImportModel>> GetRecords(Stream file, Action<ImportResult<IImportModel>> done, Action<int> progress)
        {
            // initialize a progress
            Progress = new Progress<int>(progress);

            // start the task
            return await Task.Run(() =>
            {
                using (var reader = new StreamReader(file, Encoding.Default))
                using (var csvReader = new CsvReader(reader))
                {
                    return csvReader.GetRecords<GrupeerImport>().ToList();
                }
            }).ContinueWith(Collector).ContinueWith((t) =>
            {
                // when done, inform
                done(t.Result);

                return t.Result;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }
    }
}
