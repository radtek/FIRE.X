using CsvHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace FIRE.X
{
    public partial class Form1 : Form
    {
        List<DateAmountSum> obj = new List<DateAmountSum>();

        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.DoWork += BackgroundWorker1_DoWork;
            backgroundWorker1.WorkerReportsProgress = true;
            backgroundWorker1.ProgressChanged += BackgroundWorker1_ProgressChanged;
            backgroundWorker1.RunWorkerCompleted += BackgroundWorker1_RunWorkerCompleted;
            this.cmbImport.SelectedItem = this.cmbImport.Items[0];
        }

        private void BackgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
        }

        private void BackgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            obj = (List<DateAmountSum>)e.Result;

            // set the values with the min and max we discovered from the import
            this.dateTimePicker1.Value = obj.Select(f => f.Date).Where(f => f.HasValue).Min().Value;
            this.dateTimePicker2.Value = obj.Select(f => f.Date).Where(f => f.HasValue).Max().Value;

            // reset the progressbar
            this.progressBar1.Value = 0;

            // redraw graphs
            SetCharts();

            // renable the UI
            SetEnabledWhenWorking();
        }

        private void SetDisableWhenWorking()
        {
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker2.Enabled = false;
            this.btnImport.Enabled = false;
        }

        private void SetEnabledWhenWorking()
        {
            this.dateTimePicker1.Enabled = true;
            this.dateTimePicker2.Enabled = true;
            this.btnImport.Enabled = true;
        }

        private void BackgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            var pt = (PassThrough)e.Argument;
            var data = ImportProviders.GetRecords<Mintos.Import.MintosImport>(pt.ImportProvider, pt.File);
            var dates = data.Select(x => x.Date?.Date).Distinct();

            List<DateAmountSum> obj = new List<DateAmountSum>();

            for (int i = 0; i < dates.Count(); i++)
            {
                var amount = data.Where(x => x.Date?.Date == dates.ElementAt(i) && x.Details.StartsWith("Interest income")).Sum(x => x.Turnover);
                var ob = new DateAmountSum()
                {
                    Date = dates.ElementAt(i),
                    Amount = amount,
                    Sum = amount + (i == 0 ? 0 : obj[i - 1].Sum)
                };

                obj.Add(ob);

                var percentage = ((decimal)i / dates.Count()) *100.0m;
                backgroundWorker1.ReportProgress((int)Math.Round(percentage));
            }

            e.Result = obj;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.cmbImport.SelectedItem == (object)"Mintos")
            {
                if (openFileDialogSelectImport.ShowDialog(this) == DialogResult.OK)
                {
                    // disable the UI
                    SetDisableWhenWorking();

                    backgroundWorker1.RunWorkerAsync(new PassThrough() { File = openFileDialogSelectImport.OpenFile(), ImportProvider = (string)this.cmbImport.SelectedItem });
                }
            }
        }

        private void SetCharts()
        {
            var seriePerDay = new Series();
            var serieSum = new Series();
            serieSum.ChartType = SeriesChartType.Line;
            seriePerDay.ChartType = SeriesChartType.Line;
            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].Date.HasValue && obj[i].Date.Value > this.dateTimePicker1.Value && obj[i].Date.Value < this.dateTimePicker2.Value)
                {
                    seriePerDay.Points.AddXY(obj[i].Date, obj[i].Amount);
                    serieSum.Points.AddXY(obj[i].Date, obj[i].Sum);
                }
            }
            chart1.Series.Clear();
            chart1.Series.Add(seriePerDay);
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].RecalculateAxesScale();
            chart1.Visible = true;

            chart2.Series.Clear();
            chart2.Series.Add(serieSum);
            chart2.Series[0].XValueType = ChartValueType.DateTime;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            chart2.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart2.ChartAreas[0].RecalculateAxesScale();
            chart2.Visible = true;
        }

        struct DateAmountSum
        {
            public DateTime? Date { get; set; }
            public decimal Amount { get; set; }
            public decimal Sum { get; set; }
        }

        struct PassThrough
        {
            public string ImportProvider { get; set; }
            public Stream File { get; set; }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            if(!backgroundWorker1.IsBusy)
                SetCharts();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            if (!backgroundWorker1.IsBusy)
                SetCharts();
        }

        private void btnExport1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();

            if(sfd.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(sfd.FileName, ChartImageFormat.Png);
            }
        }
    }
}
