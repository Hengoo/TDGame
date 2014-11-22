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
			//Je nachdem welche Konfiguration gewählt ist, startet sich ServerForm oder der ClientForm
#if RELEASE_CLIENT
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ClientForm());
#elif RELEASE_SERVER
			Console.WriteLine("[------- Created by Hengo -------]");
			Console.WriteLine("[----- TDGame Server DeluxE -----]");
			Console.WriteLine("[------  Press 'Q' to quit ------]");
			ServerConsole sc = new ServerConsole();
			while(Console.ReadKey().Key != ConsoleKey.Q);
			sc.Close();
#else
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new ClientForm());
#endif
			//Application.Run(new Form1());
		}
	}
}
