using System;
using System.Windows.Forms;

namespace FIRE.X.UI.Common
{
    public partial class MenuItemUserControl : Button
    {
        public MenuItemUserControl(EventHandler click = null) : base()
        {
            // register the event, when clicked we want to disable the button
            click += new EventHandler((o, args) =>
            {
                if(this.Enabled)
                    this.Enabled = false;
            });

            this.Click += click;
        }
    }
}
