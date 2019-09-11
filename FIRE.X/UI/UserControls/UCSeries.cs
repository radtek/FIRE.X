using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIRE.X.UI.UserControls
{
    public partial class UCSeries : UserControl
    {
        public UCSeries()
        {
            this.InitializeComponent();
        }

        public void AddSeries(OxyPlot.Series.Series[] series)
        {
            foreach(var serie in series)
            {
                // check if the category exists
                var categoryKey = serie.Title.Split('-')[0];
                if(!this.treeView1.Nodes.ContainsKey(categoryKey)) { 
                    this.treeView1.Nodes.Add(new TreeNode(categoryKey)
                    {
                        Name = categoryKey,
                        Checked = true
                    });
                }

                // now we know it exists, so add it
                var categoryNode = this.treeView1.Nodes[categoryKey];
                categoryNode.Nodes.Add(new TreeNode(serie.Title)
                {
                    Checked = true
                });
            }
        }

        public void Clear()
        {
            this.treeView1.Nodes.Clear();
        }

        public void Reorder()
        {

        }

        private void BtnSelectNone_Click(object sender, EventArgs e)
        {
            for(int i = 0; i < this.treeView1.Nodes.Count; i++)
            {
                this.treeView1.Nodes[i].Checked = false;
            }
        }

        private void BtnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.treeView1.Nodes.Count; i++)
            {
                this.treeView1.Nodes[i].Checked = true;
            }
        }
    }
}
