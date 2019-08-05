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
using System.Windows.Forms.DataVisualization.Charting;

namespace FIRE.X.UI.Common
{
    public partial class ChartUserControl : UserControl
    {
        private ChartUserControlView ChartUserControlView {get;set;}

        public ChartUserControl(ChartUserControlView chartUserControlView)
        {
            InitializeComponent();

            var dateRange = chartUserControlView.Chart.MaxRange();

            this.dateTimePickerFrom.Value = dateRange[0].Value;
            this.dateTimePickerTo.Value = dateRange[1].Value;

            this.dateTimePickerTo.ValueChanged += (obj, arg) => UpdateSeries(dateTimePickerFrom.Value, dateTimePickerTo.Value);
            this.dateTimePickerFrom.ValueChanged += (obj, arg) => UpdateSeries(dateTimePickerFrom.Value, dateTimePickerTo.Value);

            ChartUserControlView = chartUserControlView;

            // update the series
            UpdateSeries(dateRange[0].Value, dateRange[1].Value);

            // add the import providers
            foreach (var importProvider in ImportProviders.ImportProviderNames)
            {
                this.cmbSelectSource.Items.Add(importProvider);
            }

            // format the dropdown of importproviders
            this.cmbSelectSource.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbSelectSource.Items.Count > 0)
                this.cmbSelectSource.SelectedItem = this.cmbSelectSource.Items[0];

            // some formatting
            chart.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart.ChartAreas[0].AxisX.Interval = 1;
            chart.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            chart.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
        }

        private void UpdateSeries(DateTime from, DateTime to)
        {
            this.chart.Series.Clear();

            // add the charts
            foreach (var range in ChartUserControlView.Chart.GetSeries(from, to))
            {
                this.chart.Series.Add(range);
            }

            // format the graph based on the given range
            chart.ChartAreas[0].AxisX.IntervalType = to.Subtract(from).TotalDays > 60 ? DateTimeIntervalType.Months : DateTimeIntervalType.Days;
        }

        private void btnSaveAsImage_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "jpg";
            if(sfd.ShowDialog() == DialogResult.OK)
            {
                this.chart.SaveImage(sfd.OpenFile(), System.Windows.Forms.DataVisualization.Charting.ChartImageFormat.Png); ;
            }
        }
    }
}
