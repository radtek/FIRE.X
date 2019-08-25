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
using OxyPlot.Axes;

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

            // add category
            var categoryAxis1 = new CategoryAxis();
            foreach (var category in list)
            {
                categoryAxis1.Labels.Add(category.Key.ToString("MM-yyyy"));
            }

            model.Axes.Add(categoryAxis1);

            var maxItems = list.Select(f => f.Value.Count).Max();

            for (int i = 0; i < maxItems; i++)
            {
                var columnSeries = new ColumnSeries();
                columnSeries.IsStacked = true;
                for (int y = 0; y < list.Count(); y++)
                {
                    if(list.ElementAt(y).Value.Count > i)
                        columnSeries.Items.Add(list.ElementAt(y).Value[i]);
                    else
                        columnSeries.Items.Add(new ColumnItem(0));
                }
                model.Series.Add(columnSeries);

            }
            

            this.plotView1.Model = model;
        }
    }
}
