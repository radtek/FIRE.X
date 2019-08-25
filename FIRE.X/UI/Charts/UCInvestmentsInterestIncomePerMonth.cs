using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FIRE.X.DL;
using OxyPlot;
using OxyPlot.Series;

namespace FIRE.X.UI.Charts
{
    public partial class UCInvestmentsInterestIncomePerMonth : UserControl
    {
        public UCInvestmentsInterestIncomePerMonth()
        {
            InitializeComponent();

            if(System.Diagnostics.Process.GetCurrentProcess().ProcessName != "devenv")
            {
                GetData();
            }
        }

        public async void GetData()
        {
            var data = await Task.Run(() =>
            {
                return ContextHelpers.InvestmentInterestPerMonthPerYearPerSource();
            });

            var model = new PlotModel();

            var list = new Dictionary<DateTime, List<ColumnItem>>();
            foreach(var da in data)
            {
                var key = new DateTime(da.Year, da.Month, 1);

                if(list.ContainsKey(key))
                {
                    var values = list[key];
                    values.Add(new ColumnItem((double)da.Amount));
                } else
                {
                    list.Add(key, new List<ColumnItem>() { new ColumnItem((double)da.Amount) });
                }
            }

            foreach(var item in list)
            {
                var columnSeries = new ColumnSeries();
                columnSeries.IsStacked = true;
                columnSeries.Items.AddRange(item.Value);
                model.Series.Add(columnSeries);
            }

            this.plotView1.Model = model;

            //foreach(var d in data)
            //{
            //    series.Slices.Add(new PieSlice(d.Source.ToString(), (double)d.Amount));
            //}

            //model.Series.Add(series);

            //this.plotView1.Model = model;
        }
    }
}
