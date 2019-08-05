using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FIRE.X.UI.Common
{
    public partial class MenuUserControl : UserControl
    {
        public MenuUserControlView MenuUserControlView { get; }

        public MenuUserControl(MenuUserControlView menuUserControlView)
        {
            this.MenuUserControlView = menuUserControlView;

            InitializeComponent();

            if (menuUserControlView.MenuItems.Any())
            {
                var tlp = new TableLayoutPanel();
                foreach (var button in menuUserControlView.MenuItems)
                {
                    var index = tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize, button.Height));
                    tlp.Controls.Add(button, 0, index);
                }

                this.Controls.Add(tlp);
            }
        }

        /// <summary>
        ///     Register the events so that every button is enabled when another has been clicked
        /// </summary>
        public MenuUserControl RegisterEvents()
        {
            foreach(var menuItem in MenuUserControlView.MenuItems)
            {
                menuItem.Click += (obj, args) =>
                {
                    foreach(var item in MenuUserControlView.MenuItems.Where(i => i != menuItem))
                    {
                        item.Enabled = true;
                    }
                };
            }

            return this;
        }

    }
}
