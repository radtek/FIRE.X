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
            this.btnSaveAsImage = new System.Windows.Forms.Button();
            this.plotView1 = new OxyPlot.WindowsForms.PlotView();
            this.btnReset = new System.Windows.Forms.Button();
            this.ucSeries = new FIRE.X.UI.UserControls.UCSeries();
            this.SuspendLayout();
            // 
            // btnSaveAsImage
            // 
            this.btnSaveAsImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveAsImage.Location = new System.Drawing.Point(764, 3);
            this.btnSaveAsImage.Name = "btnSaveAsImage";
            this.btnSaveAsImage.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAsImage.TabIndex = 4;
            this.btnSaveAsImage.Text = "Export";
            this.btnSaveAsImage.UseVisualStyleBackColor = true;
            this.btnSaveAsImage.Click += new System.EventHandler(this.btnSaveAsImage_Click);
            // 
            // plotView1
            // 
            this.plotView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plotView1.BackColor = System.Drawing.Color.White;
            this.plotView1.Location = new System.Drawing.Point(3, 29);
            this.plotView1.Name = "plotView1";
            this.plotView1.PanCursor = System.Windows.Forms.Cursors.Hand;
            this.plotView1.Size = new System.Drawing.Size(610, 353);
            this.plotView1.TabIndex = 5;
            this.plotView1.Text = "plotView1";
            this.plotView1.ZoomHorizontalCursor = System.Windows.Forms.Cursors.SizeWE;
            this.plotView1.ZoomRectangleCursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.plotView1.ZoomVerticalCursor = System.Windows.Forms.Cursors.SizeNS;
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.Location = new System.Drawing.Point(683, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(75, 23);
            this.btnReset.TabIndex = 8;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // ucSeries
            // 
            this.ucSeries.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucSeries.Location = new System.Drawing.Point(619, 29);
            this.ucSeries.Name = "ucSeries";
            this.ucSeries.Size = new System.Drawing.Size(223, 353);
            this.ucSeries.TabIndex = 7;
            // 
            // ChartUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.ucSeries);
            this.Controls.Add(this.btnSaveAsImage);
            this.Controls.Add(this.plotView1);
            this.Name = "ChartUserControl";
            this.Size = new System.Drawing.Size(842, 385);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnSaveAsImage;
        private OxyPlot.WindowsForms.PlotView plotView1;
        private UserControls.UCSeries ucSeries;
        private System.Windows.Forms.Button btnReset;
    }
}
