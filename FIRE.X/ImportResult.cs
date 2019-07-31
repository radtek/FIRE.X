using System;
using System.Collections.Generic;

namespace FIRE.X
{
    public class ImportResult<T>
    {
        public List<T> ImportRules { get; set; }
        public List<DateAmountSum> ChartData { get; set; }
    }
}
