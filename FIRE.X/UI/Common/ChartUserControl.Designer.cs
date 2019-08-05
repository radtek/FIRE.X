namespace FIRE.X.UI.Common
{
    partial class ChartUserControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.dateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.cmbSelectSource = new System.Windows.Forms.ComboBox();
            this.btnSaveAsImage = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.SuspendLayout();
            // 
            // chart
            // 
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.AxisX.LabelStyle.Format = "yyyy-MM-dd";
            chartArea1.AxisX.LabelStyle.Interval = 1D;
            chartArea1.AxisX.LabelStyle.IntervalOffset = 1D;
            chartArea1.AxisX.LabelStyle.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Months;
            chartArea1.Name = "ChartArea1";
            this.chart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart.Legends.Add(legend1);
            this.chart.Location = new System.Drawing.Point(0, 29);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(842, 356);
            this.chart.TabIndex = 0;
            this.chart.Text = "chart1";
            // 
            // dateTimePickerFrom
            // 
            this.dateTimePickerFrom.Location = new System.Drawing.Point(3, 3);
            this.dateTimePickerFrom.Name = "dateTimePickerFrom";
            this.dateTimePickerFrom.Size = new System.Drawing.Size(197, 20);
            this.dateTimePickerFrom.TabIndex = 1;
            // 
            // dateTimePickerTo
            // 
            this.dateTimePickerTo.Location = new System.Drawing.Point(206, 3);
            this.dateTimePickerTo.Name = "dateTimePickerTo";
            this.dateTimePickerTo.Size = new System.Drawing.Size(200, 20);
            this.dateTimePickerTo.TabIndex = 2;
            // 
            // cmbSelectSource
            // 
            this.cmbSelectSource.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSelectSource.FormattingEnabled = true;
            this.cmbSelectSource.Location = new System.Drawing.Point(718, 3);
            this.cmbSelectSource.Name = "cmbSelectSource";
            this.cmbSelectSource.Size = new System.Drawing.Size(121, 21);
            this.cmbSelectSource.TabIndex = 3;
            // 
            // btnSaveAsImage
            // 
            this.btnSaveAsImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAsImage.Location = new System.Drawing.Point(637, 3);
            this.btnSaveAsImage.Name = "btnSaveAsImage";
            this.btnSaveAsImage.Size = new System.Drawing.Size(75, 21);
            this.btnSaveAsImage.TabIndex = 4;
            this.btnSaveAsImage.Text = "Export";
            this.btnSaveAsImage.UseVisualStyleBackColor = true;
            this.btnSaveAsImage.Click += new System.EventHandler(this.btnSaveAsImage_Click);
            // 
            // ChartUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnSaveAsImage);
            this.Controls.Add(this.cmbSelectSource);
            this.Controls.Add(this.dateTimePickerTo);
            this.Controls.Add(this.dateTimePickerFrom);
            this.Controls.Add(this.chart);
            this.Name = "ChartUserControl";
            this.Size = new System.Drawing.Size(842, 385);
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.DateTimePicker dateTimePickerFrom;
        private System.Windows.Forms.DateTimePicker dateTimePickerTo;
        private System.Windows.Forms.ComboBox cmbSelectSource;
        private System.Windows.Forms.Button btnSaveAsImage;
    }
}
