using System;
using System.Windows.Forms;

namespace FractalPainting.App
{
    public static class Program
    {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            try
            {
              Application.EnableVisualStyles();
              Application.SetCompatibleTextRenderingDefault(false);
              Application.Run(DIContainerTask.CreateMainForm());
            }
            catch (Exception e)
            {
                MessageBox.Show("Use ctrl + c to copy stacktrace to clipboard\n\n" + e, "ERROR!");
            }
        }
    }
}