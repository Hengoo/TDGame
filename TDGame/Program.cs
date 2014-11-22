using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TDGame
{
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Je nachdem was auskommentiert ist, startet sich der Server(Form2) oder der Client(Form1)
            Application.Run(new Form2());
            //Application.Run(new Form1());
        }
    }
}
