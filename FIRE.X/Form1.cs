using CsvHelper;
using FIRE.X.Properties;
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
        bool InitialSetup = true;

        public Form1()
        {
            InitializeComponent();
            
            this.cmbImport.SelectedItem = this.cmbImport.Items[0];
            this.Text = $"{this.Text} - {System.Reflection.Assembly.GetExecutingAssembly().GetName().Version}";
        }

        private void DoneImporting()
        {
            // reset the progressbar
            this.progressBar1.Value = 0;

            // redraw graphs
            SetCharts();

            // set checkboxes
            this.checkedListBox1.Items.AddRange(new string[] { Resources.RENT_DAY, Resources.RENT_TOTAL, Resources.BALANCE, Resources.INVESTMENTS, Resources.PRINCIPAL });
            this.checkedListBox1.SetItemChecked(0, true);
            this.checkedListBox1.SetItemChecked(1, true);
            this.checkedListBox1.SetItemChecked(2, true);
            this.checkedListBox1.SetItemChecked(3, true);
            this.checkedListBox1.SetItemChecked(4, true);

            // set the values with the min and max we discovered from the import
            this.dateTimePicker1.Value = obj.Select(f => f.Date).Where(f => f.HasValue).Min().Value;
            this.dateTimePicker2.Value = obj.Select(f => f.Date).Where(f => f.HasValue).Max().Value;

            // renable the UI
            SetEnabledWhenWorking();

            // Done setup
            InitialSetup = false;

            // Update
            UpdateCharts();
        }

        /// <summary>
        ///     Disable the UI
        /// </summary>
        private void SetDisableWhenWorking()
        {
            this.dateTimePicker1.Enabled = false;
            this.dateTimePicker2.Enabled = false;
            this.btnImport.Enabled = false;
            this.btnExport1.Enabled = false;
        }

        /// <summary>
        ///     Enable the UI
        /// </summary>
        private void SetEnabledWhenWorking()
        {
            this.dateTimePicker1.Enabled = true;
            this.dateTimePicker2.Enabled = true;
            this.btnImport.Enabled = true;

            this.btnExport1.Visible = true;

            this.btnExport1.Enabled = true;

            this.dateTimePicker1.Visible = true;
            this.dateTimePicker2.Visible = true;
        }
        
        private void btnImport_Click(object sender, EventArgs e)
        {
            if (this.cmbImport.SelectedItem == (object)"Mintos")
            {
                if (openFileDialogSelectImport.ShowDialog(this) == DialogResult.OK)
                {
                    var pt = new PassThrough() { File = openFileDialogSelectImport.OpenFile(), ImportProvider = (string)this.cmbImport.SelectedItem };
                    var importProvider = ImportProviders.GetImportProvider(pt.ImportProvider);
                    importProvider.GetRecords<Mintos.Import.MintosImport>(pt.File, (t) =>
                    {
                        obj = t.ChartData;
                        DoneImporting();
                    }, (p) =>
                    {
                        this.progressBar1.Value = p;
                    });
                }
            }
        }

        private void UpdateCharts()
        {
            // Initial setup don't show it
            if (InitialSetup)
                return;

            chart1.Series[Resources.RENT_DAY].Points.Clear();
            chart1.Series[Resources.RENT_TOTAL].Points.Clear();
            chart1.Series[Resources.BALANCE].Points.Clear();
            chart1.Series[Resources.INVESTMENTS].Points.Clear();
            chart1.Series[Resources.PRINCIPAL].Points.Clear();

            if (checkedListBox1.CheckedItems.Contains(Resources.RENT_DAY))
                chart1.Series[Resources.RENT_DAY].Enabled = true;
            else
            {
                chart1.Series[Resources.RENT_DAY].Enabled = false;
            }

            if (checkedListBox1.CheckedItems.Contains(Resources.RENT_TOTAL))
                chart1.Series[Resources.RENT_TOTAL].Enabled = true;
            else
            {
                chart1.Series[Resources.RENT_TOTAL].Enabled = false;
            }

            if (checkedListBox1.CheckedItems.Contains(Resources.BALANCE))
                chart1.Series[Resources.BALANCE].Enabled = true;
            else
            {
                chart1.Series[Resources.BALANCE].Enabled = false;
            }

            if (checkedListBox1.CheckedItems.Contains(Resources.INVESTMENTS))
                chart1.Series[Resources.INVESTMENTS].Enabled = true;
            else
            {
                chart1.Series[Resources.INVESTMENTS].Enabled = false;
            }

            if (checkedListBox1.CheckedItems.Contains(Resources.PRINCIPAL))
                chart1.Series[Resources.PRINCIPAL].Enabled = true;
            else
            {
                chart1.Series[Resources.PRINCIPAL].Enabled = false;
            }

            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].Date.HasValue && obj[i].Date.Value >= this.dateTimePicker1.Value && obj[i].Date.Value <= this.dateTimePicker2.Value)
                {
                    if(chart1.Series[Resources.RENT_DAY].Enabled)
                        chart1.Series[Resources.RENT_DAY].Points.AddXY(obj[i].Date, obj[i].Amount);

                    if (chart1.Series[Resources.RENT_TOTAL].Enabled)
                        chart1.Series[Resources.RENT_TOTAL].Points.AddXY(obj[i].Date, obj[i].Sum);


                    if (chart1.Series[Resources.BALANCE].Enabled)
                        chart1.Series[Resources.BALANCE].Points.AddXY(obj[i].Date, obj[i].Balance);

                    if (chart1.Series[Resources.INVESTMENTS].Enabled)
                        chart1.Series[Resources.INVESTMENTS].Points.AddXY(obj[i].Date, obj[i].Investment);

                    if (chart1.Series[Resources.PRINCIPAL].Enabled)
                        chart1.Series[Resources.PRINCIPAL].Points.AddXY(obj[i].Date, obj[i].Principal);
                }
            }

            if(this.dateTimePicker2.Value.Subtract(this.dateTimePicker1.Value).TotalDays > 60)
            {
                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            } else
            {
                chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Days;
            }
        }

        private void SetCharts()
        {
            var seriePerDay = new Series(Resources.RENT_DAY);
            var serieSum = new Series(Resources.RENT_TOTAL);
            var serieBalance = new Series(Resources.BALANCE);
            var serieInvestments = new Series(Resources.INVESTMENTS);
            var seriePrincipal = new Series(Resources.PRINCIPAL);

            seriePerDay.ChartType = SeriesChartType.Column;
            serieSum.ChartType = SeriesChartType.StepLine;
            serieBalance.ChartType = SeriesChartType.StepLine;
            serieInvestments.ChartType = SeriesChartType.Column;
            seriePrincipal.ChartType = SeriesChartType.Column;

            for (int i = 0; i < obj.Count; i++)
            {
                if (obj[i].Date.HasValue && obj[i].Date.Value > this.dateTimePicker1.Value && obj[i].Date.Value < this.dateTimePicker2.Value)
                {
                    seriePerDay.Points.AddXY(obj[i].Date, obj[i].Amount);
                    serieSum.Points.AddXY(obj[i].Date, obj[i].Sum);
                    serieBalance.Points.AddXY(obj[i].Date, obj[i].Balance);
                    serieInvestments.Points.AddXY(obj[i].Date, obj[i].Investment);
                    seriePrincipal.Points.AddXY(obj[i].Date, obj[i].Principal);
                }
            }
            
            chart1.Series.Clear();
            chart1.Series.Add(seriePerDay);
            chart1.Series.Add(serieSum);
            chart1.Series.Add(serieBalance);
            chart1.Series.Add(serieInvestments);
            chart1.Series.Add(seriePrincipal);
            chart1.Series[0].XValueType = ChartValueType.DateTime;
            chart1.ChartAreas[0].AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].AxisX.IntervalType = DateTimeIntervalType.Months;
            chart1.ChartAreas[0].AxisX.IntervalOffset = 1;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.Visible = true;
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            UpdateCharts();
        }

        private void dateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            UpdateCharts();
        }

        private void btnExport1_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.Filter = "PNG image (*.png)|*.png";
            sfd.DefaultExt = "png";
            sfd.AddExtension = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                chart1.SaveImage(sfd.FileName, ChartImageFormat.Png);
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            CheckedListBox clb = (CheckedListBox)sender;
            // Switch off event handler
            clb.ItemCheck -= checkedListBox1_ItemCheck;
            clb.SetItemCheckState(e.Index, e.NewValue);
            // Switch on event handler
            clb.ItemCheck += checkedListBox1_ItemCheck;

            UpdateCharts();
        }
    }
}
