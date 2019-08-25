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
                Balance = Balance,
                TransactionType = GetTransactionType(this.Details)
            };
        }

        private string GetLoanIdFromDetails()
        {
            string search = "Loan ID: ";
            int idx = Details.IndexOf(search);
            if (idx == -1)
                return String.Empty;
            else
                return Details.Substring(Details.IndexOf(search) + search.Length);
        }

        private TransactionType GetTransactionType(string details)
        {
            if (details.StartsWith("Incoming client payment"))
                return TransactionType.Deposit;
            else if (details.StartsWith("Investment principal increase Loan ID:"))
                return TransactionType.Investment;
            else if (
                details.ToLower().Contains("interest"))
                return TransactionType.Interest;
            else if (details.StartsWith("Investment principal rebuy Rebuy purpose:") || details.StartsWith("Investment principal repayment Loan ID:"))
                return TransactionType.Principal;
            else if (details.StartsWith("Withdraw"))
                return TransactionType.Withdraw;
            else
                return TransactionType.Other;
        }
    }
}
