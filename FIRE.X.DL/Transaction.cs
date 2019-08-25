using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIRE.X.DL
{
    public class Transaction
    {
        public int Id { get; set; }

        [Index("IX_UniqueTransaction", 1, IsUnique = true)]
        public TransactionSource Source { get; set; }

        [Index("IX_UniqueTransaction", 2, IsUnique = true)]
        public string TransactionId { get; set; }

        public decimal Amount { get; set; }

        public DateTime? Date { get; set; }

        public string LoanId { get; set; }

        public decimal? Balance { get; set; }
        public TransactionType TransactionType { get; set; }
    }

}
