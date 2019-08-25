using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using FIRE.X.DL;
using System.Threading.Tasks;

namespace FIRE.X.UI.Common
{
    public partial class P2PImportUserControl : UserControl
    {
        public P2PImportUserControl()
        {
            InitializeComponent();

            foreach(var importProvider in ImportProviders.ImportProviderNames)
            {
                this.cmbImport.Items.Add(importProvider);
            }

            // format the dropdown of importproviders
            this.cmbImport.DropDownStyle = ComboBoxStyle.DropDownList;
            if (cmbImport.Items.Count > 0)
                this.cmbImport.SelectedItem = this.cmbImport.Items[0];

            this.btnSelectFile.Click += BtnSelectFile_Click;
        }

        private List<IImportModel> Data;

        private void BtnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogSelectImport.ShowDialog(this) == DialogResult.OK)
            {
                this.lblPercentage.Visible = true;
                var pt = new PassThrough() { File = openFileDialogSelectImport.OpenFile(), ImportProvider = cmbImport.SelectedItem as string };
                var importProvider = ImportProviders.GetImportProvider(pt.ImportProvider);
                importProvider.GetRecords(pt.File, (data) =>
                {
                    // set the import result
                    Data = data.ImportRules.ToList();

                    var ds = new BindingSource();
                    ds.DataSource = data.ImportRulesAsDataTable();

                    this.dataGridView1.DataSource = ds;
                    this.progressBar1.Value = 0;
                    this.lblPercentage.Visible = false;

                    if (data.ImportRules.Count > 0)
                        this.btnImport.Visible = true;
                }, (p) =>
                {
                    this.lblPercentage.Text = p.ToString();
                    this.progressBar1.Value = p;
                });
            }
        }

        private async void btnImport_Click(object sender, EventArgs e)
        {
            IProgress<decimal> p = new Progress<decimal>(ImportProgressUpdate);

            btnImport.Enabled = false;
            await Task.Run(() =>
            {
                var list = new List<Transaction>();
                for (int i = 0; i < Data.Count; i++) 
                {
                    list.Add(Data[i].AsTransaction());

                    p.Report((decimal)i / Data.Count * 100.0m);
                }

                return list;
            })
            .ContinueWith(f =>
            {
                return ContextHelpers.AddTransactions(f.Result.ToArray());
            }).Unwrap()
            .ContinueWith(v =>
            {
                btnImport.Enabled = true;
                this.lblPercentage.Text = string.Empty;
                this.progressBar1.Value = 0;
            }, TaskScheduler.FromCurrentSynchronizationContext());
        }

        private void ImportProgressUpdate(decimal progress)
        {
            this.lblPercentage.Text = Math.Round(progress).ToString();
            this.progressBar1.Value = (int)progress;
        }
    }
}
