using System;

namespace FIRE.X
{

    public struct DateAmountSum
    {
        public DateTime? Date { get; set; }
        public decimal Amount { get; set; }
        public decimal Sum { get; set; }
        public decimal Balance { get; set; }
        public decimal Investment { get; set; }
        public decimal Principal { get; set; }
    }
}
