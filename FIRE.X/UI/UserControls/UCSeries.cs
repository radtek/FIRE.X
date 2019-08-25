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
                this.checkedListBox1.Items.Add(serie.Title, true);
            }
        }

        public void Clear()
        {
            this.checkedListBox1.Items.Clear();
        }
    }
}
