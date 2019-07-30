using CsvHelper.Configuration.Attributes;
using System;
namespace FIRE.X.Mintos.Import
{
    public class MintosImport : IImportModel
    {
        [Name("Transaction ID")]
        public int? TransactionID { get; set; }

        [Name("Date", "Next Payment")]
        public DateTime? Date { get; set; }

        public string Details { get; set; }

        [Name("Turnover", "Estimated")]
        public decimal Turnover { get; set; }

        public decimal Balance { get; set; }

        public string Currency { get; set; }
    }
}
