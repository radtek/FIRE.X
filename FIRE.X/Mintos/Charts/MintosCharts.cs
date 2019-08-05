using FIRE.X.DL;
using FIRE.X.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms.DataVisualization.Charting;

namespace FIRE.X.Mintos.Charts
{
    public class MintosCharts : IChart
    {
        public TransactionSource GetTransactionSource() => TransactionSource.Mintos;

        public Series[] GetSeries(DateTime from, DateTime to)
        {
            var data = ContextHelpers.GetTransactions(GetTransactionSource(), from, to);

            var seriePerDay = new Series(Resources.RENT_DAY);
            //var serieSum = new Series(Resources.RENT_TOTAL);
            var serieBalance = new Series(Resources.BALANCE);
            //var serieInvestments = new Series(Resources.INVESTMENTS);
            //var seriePrincipal = new Series(Resources.PRINCIPAL);

            seriePerDay.ChartType = SeriesChartType.Column;
            //serieSum.ChartType = SeriesChartType.StepLine;
            serieBalance.ChartType = SeriesChartType.StepLine;
            //serieInvestments.ChartType = SeriesChartType.Column;
            //seriePrincipal.ChartType = SeriesChartType.Column;

            for (int i = 0; i < data.Count; i++)
            {
                if (data[i].Date.HasValue && data[i].Date.Value >= from && data[i].Date.Value <= to)
                {
                    seriePerDay.Points.AddXY(data[i].Date, data[i].Amount);
                    //serieSum.Points.AddXY(data[i].Date, sum);
                    serieBalance.Points.AddXY(data[i].Date, data[i].Balance);
                    //serieInvestments.Points.AddXY(data[i].Date, data[i].Investment);
                    //seriePrincipal.Points.AddXY(data[i].Date, data[i].Principal);
                }
            }

            return new Series[2] {
                seriePerDay,
                //serieSum,
                serieBalance,
                //serieInvestments,
                //seriePrincipal
            };
        }

        public DateTime?[] MaxRange()
        {
            return ContextHelpers.GetRange()[GetTransactionSource()];
        }
    }
}
