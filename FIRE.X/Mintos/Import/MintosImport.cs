using CsvHelper.Configuration.Attributes;
using FIRE.X.DL;
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

        /// <summary>
        ///     Convert the object a transaction
        /// </summary>
        /// <returns></returns>
        public Transaction AsTransaction()
        {
            return new Transaction()
            {
                Amount = Turnover,
                Date = Date,
                Source = TransactionSource.Mintos,
                TransactionId = TransactionID,
                LoanId = GetLoanIdFromDetails(),
                Balance = Balance
            };
        }

        private string GetLoanIdFromDetails()
        {
            string search = "Loan ID: ";
            return Details.Substring(Details.IndexOf(search) + search.Length);
        }
    }
}
