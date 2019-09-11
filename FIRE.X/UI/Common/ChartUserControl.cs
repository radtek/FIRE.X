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
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;

namespace FIRE.X.UI.Common
{
    public partial class ChartUserControl : UserControl
    {
        private ChartUserControlView ChartUserControlView { get; set; }

        private PlotModel Chart { get; set; }

        public ChartUserControl(ChartUserControlView chartUserControlView)
        {
            InitializeComponent();

            Chart = new PlotModel();
            Chart.LegendOrientation = LegendOrientation.Horizontal;
            Chart.LegendPlacement = LegendPlacement.Outside;
            Chart.LegendPosition = LegendPosition.BottomLeft;
            Chart.IsLegendVisible = true;
            Chart.Series.CollectionChanged += Series_CollectionChanged;

            var xAxis = new DateTimeAxis();
            xAxis.StringFormat = "yyyy-MM-dd";
            xAxis.Position = AxisPosition.Bottom;
            xAxis.IntervalLength = 1;
            xAxis.IntervalType = OxyPlot.Axes.DateTimeIntervalType.Months;
            xAxis.MinorIntervalType = OxyPlot.Axes.DateTimeIntervalType.Days;

            Chart.Axes.Add(xAxis);

            // set the viewmodel
            this.ChartUserControlView = chartUserControlView;

            this.plotView1.Model = Chart;

            this.ucSeries.treeView1.AfterCheck += TreeView1_AfterCheck;

            LoadAll();
        }

        private void TreeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            // dont do anything when we set the sub items on checked state of the parent
            if (e.Action == TreeViewAction.Unknown)
                return;

            // unselect the children if any
            foreach (var node in e.Node.Nodes)
            {
                (node as TreeNode).Checked = e.Node.Checked;
            }

            List<string> checkedItems = GetChecked(e.Node, true);
            
            //foreach (var serie in this.Chart.Series)
            //{
            //    if (!checkedItems.Contains(serie.Title))
            //        serie.IsVisible = false;
            //    else
            //        serie.IsVisible = true;
            //}

            this.Chart.InvalidatePlot(true);
        }

        public List<string> GetChecked(TreeNode node, bool lookUp = false)
        {
            var _list = new List<string>();

            if(node.Nodes.Count > 0)
            {
                foreach(var _node in node.Nodes.Cast<TreeNode>().Where(n => n.Checked))
                    _list.AddRange(GetChecked(_node, true));
            }

            if (lookUp)
            {
                if (node.Parent != null && node.Parent.Nodes.Count > 0)
                {
                    _list.AddRange(GetChecked(node.Parent, true));
                }

                if (node.PrevNode != null)
                    _list.AddRange(GetChecked(node.PrevNode));

                if (node.NextNode != null)
                    _list.AddRange(GetChecked(node.NextNode));

            }



            if(node.Checked)
                _list.Add(node.Text);

            return _list;
        }

        private void Series_CollectionChanged(object sender, ElementCollectionChangedEventArgs<OxyPlot.Series.Series> e)
        {
            ucSeries.AddSeries(e.AddedItems.ToArray());
            ucSeries.Reorder();
        }

        private void Clear()
        {
            this.Chart.Series.Clear();
            this.ucSeries.Clear();
        }

        private async void UpdateSeries(DateTime from, DateTime to)
        {
            // we still need the min and max information
            var minMax = ChartUserControlView.Chart.MaxRange();

            // add the charts
            await ChartUserControlView.Chart.GetSeries(from, to).ContinueWith(r =>
            {
                foreach (var range in r.Result)
                {
                    this.Chart.Series.Add((LineSeries)range);
                }
                this.Chart.InvalidatePlot(true);
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private async void UpdateSeries(IChart chart)
        {
            var charts = chart;

            var range = charts.MaxRange();
            var results = await charts.GetSeries(range[0].Value, range[1].Value);

            foreach (var lineSerie in results.Cast<LineSeries>())
            {
                this.Chart.Series.Add(lineSerie);
            }
            this.Chart.InvalidatePlot(true);
        }

        private void btnSaveAsImage_Click(object sender, EventArgs e)
        {
            var sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "jpg";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                new SvgExporter().Export(this.Chart, sfd.OpenFile());
            }
        }

        private void LoadAll()
        {
            this.Clear();
            UpdateSeries(new FIRE.X.Charts.Charts());
            UpdateSeries(new Mintos.Charts.MintosCharts());
            UpdateSeries(new Grupeer.Charts.GrupeerCharts());
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            Chart.ResetAllAxes();
            Chart.InvalidatePlot(true);
        }
    }
}