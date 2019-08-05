using FIRE.X.DL;
using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace FIRE.X
{
    public interface IChart
    {
        TransactionSource GetTransactionSource();
        Series[] GetSeries(DateTime from, DateTime to);
        DateTime?[] MaxRange();
    }
}
