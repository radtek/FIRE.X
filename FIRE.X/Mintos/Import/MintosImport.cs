using CsvHelper.Configuration.Attributes;
using System;
namespace FIRE.X.Mintos.Import
{
    public class MintosImport : IImportModel
    {
        [Name("Transaction ID", "ID")]
        public string TransactionID { get; set; }

        [Name("Date", "Next Payment")]
        public DateTime? Date { get; set; }

        [Optional()]
        public string Details { get; set; }

        [Name("Turnover", "Estimated Next Payment")]
        public decimal Turnover { get; set; }

        [Optional()]
        public decimal Balance { get; set; }

        public string Currency { get; set; }
    }
}
