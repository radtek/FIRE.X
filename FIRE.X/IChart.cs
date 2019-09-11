using FIRE.X.DL;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace FIRE.X
{
    public interface IChart
    {
        Expression<Func<Transaction, bool>> GetTransactionSource();
        Task<object[]> GetSeries(DateTime from, DateTime to);
        DateTime?[] MaxRange();
        string GetLineSeriesName(string serieName);
    }
}
