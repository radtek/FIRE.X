using System;
using System.Windows.Forms;

namespace FIRE.X
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initial settings
            ImportProviders.RegisterImportProviders();

            // Run the application
            Application.Run(new Form1());
        }
    }
}
