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
    public partial class UCInvestmentPerSource : UserControl
    {
        public UCInvestmentPerSource()
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
                return ContextHelpers.InvestmentsPerSource();
            });

            var model = new PlotModel();
            var series = new PieSeries()
            {
                StrokeThickness = 2.0,
                InsideLabelPosition = 0.5,
                AngleSpan = 360,
                StartAngle = 0,
                InnerDiameter = 0.4
            };

            foreach(var d in data)
            {
                series.Slices.Add(new PieSlice(d.Source.ToString(), (double)d.Amount));
            }

            model.Series.Add(series);

            this.plotView1.Model = model;
        }
    }
}
