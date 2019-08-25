using System;
using FIRE.X.DL;

namespace FIRE.X.Grupeer.Import
{
    public class GrupeerImport : IImportModel
    {
        public DateTime Date { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public decimal Balance { get; set; }
        public string Currency { get; set; }
        public int? GRP { get; set; }

        public Transaction AsTransaction()
        {
            return new Transaction()
            {
                Amount = Amount,
                Balance = Balance,
                TransactionId = GetTransactionId(),
                Source = TransactionSource.Grupeer,
                Date = Date,
                LoanId = GRP.ToString() ?? String.Empty,
                TransactionType = GetTransactionType()
            };
        }
        private string GetTransactionId()
        {
            return $"{Date.ToString("ddMMyyyy")}-{Amount}-{Description}";
        }

        private TransactionType GetTransactionType()
        {
            if (Type == "Deposit")
                return TransactionType.Deposit;
            else if (Type == "Investment")
                return TransactionType.Investment;
            else if (Type == "Interest")
                return TransactionType.Interest;
            else if (Type == "Principal")
                return TransactionType.Principal;
            else
                return TransactionType.Other;
        }
    }
}
