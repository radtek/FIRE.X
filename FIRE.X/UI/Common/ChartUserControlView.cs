using FIRE.X.UI.UserControls;

namespace FIRE.X.UI.Common
{
    public class ChartUserControlView
    {
        public IChart Chart { get; set; }

        public ChartUserControlView(IChart chart = null)
        {
            this.Chart = chart;
        }
    }
}