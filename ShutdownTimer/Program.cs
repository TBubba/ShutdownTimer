using System;
using System.Windows.Forms;
using System.Collections.Generic;
using ShutdownTimer.Options;

namespace ShutdownTimer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // Get and parse command line arguments
            CLArgs.ParseArgs();

            // Start application
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
