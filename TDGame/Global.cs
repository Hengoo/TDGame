using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace TDGame
{
	/// <summary>
	/// Für die unterscheidung zwischen verschiedenen projektieltypen
	/// </summary>
	public enum Projektileffekt : byte
	{
		/// <summary>
		/// Verursacht schaden beim aufprall
		/// </summary>
		schaden,
		/// <summary>
		/// versorft den turm mit energie beim aufprall
		/// </summary>
		energie,
		/// <summary>
		/// erhöt den schildwert beim aufprall
		/// </summary>
		schild,
		/// <summary>
		/// setzt die Momentane Abklingzeit des Turms auf 0
		/// </summary>
		angriffsgeschwindigkeit
	}

	/// <summary>
	/// Zur erkennung und übergebung von upgradetypen
	/// </summary>
	public enum Typ : byte
	{
		/// <summary>
		/// Das Deffensive wall upgrade
		/// </summary>
		wall,
		/// <summary>
		/// Das offensive turm upgrade
		/// </summary>
		turm,
		/// <summary>
		/// Das spezielle spezial upgrade
		/// </summary>
		spezial
	}
	/// <summary>
	/// Enumeration zur unterscheidung der verschiedenen Tastendrücke
	/// </summary>
	public enum Tasten : byte
	{
		/// <summary>
		/// Tastendruck Wallverbesserung
		/// </summary>
		button1,
		/// <summary>
		/// Tastendruck Turmverbesserung
		/// </summary>
		button2,
		/// <summary>
		/// Tastendruck Spezialverbesserung
		/// </summary>
		button3,
		/// <summary>
		/// Tastendruck WallStatverbesserung
		/// </summary>
		button4,
		/// <summary>
		/// Tastendruck TurmStatverbesserung
		/// </summary>
		button5,
		/// <summary>
		/// Tastendruck SpezialStatverbesserung
		/// </summary>
		button6
	}

	/// <summary>
	/// Wird verwendet um anzuzeigen welchem Spieler ein Gebaude oder eine Kugel gehört
	/// </summary>
	public enum Spieler : byte
	{
		/// <summary>
		/// Neutrale, nicht zerstörbare Spielfeldobjekte(berge)
		/// </summary>
		umgebung,
		/// <summary>
		/// Spielfeldobjekte des Spielers A
		/// </summary>
		spielerA,
		/// <summary>
		/// Spielfeldobjekte des Spielers B
		/// </summary>
		spielerB
	}

	/// <summary>
	/// Wird verwendet um unter verschiedenen Befehlstypen bei der Übertragung zu unterscheiden
	/// </summary>
	public enum Befehlstyp : byte
	{
		/// <summary>
		/// Befehlstyp Um ein Gebaude zu erschaffen oder zu verbessern. Danach müssen die bytes für x,y Koordinaten, der Verbesserungstyp, und der Spieler folgen.
		/// </summary>
		verbessern,
		/// <summary>
		/// Befehlstyp um die Stats eines Gebaude zu verbessern. Danach müssen die bytes für x,y Koordinaten, der Verbesserungstyp, und der Spieler folgen.
		/// </summary>
		verbessernStats,
		/// <summary>
		/// Befehlstyp um einen Turm auszurichten. Danach müssen die bytes für x,y Koordinaten, 8 bytes für die Ausrichtung, und der Spieler folgen
		/// </summary>
		turmDrehen,
		/// <summary>
		/// Wird vom Server verwendet um den Clienten mitzuteilen das Blöcke zerstört wurden. Danach müssen die Bytes für x,y Koordinaten und dem Spieler folgen
		/// </summary>
		turmKaput,
		/// <summary>
		/// beendet den baumodus, und startet den Zielmodus.
		/// </summary>
		zielmodus,
		/// <summary>
		/// Der baumodus beginnt da alle Spieler mit dem server verbunden sind
		/// </summary>
		baumodus,
		/// <summary>
		/// der Kampfmodus beginnt
		/// </summary>
		kampfmodus,
		/// <summary>
		/// Beendet das spiel weil die Basis down ist.
		/// </summary>
		ende,
		/// <summary>
		/// Befehlstyp um Beziertürme auszurichten.Danach müssen Spieler, P1.X, P1.Y, P2.X, P2.Y, P3.X, P3.Y, P4.X, P4.X
		/// </summary>
		bezierAusrichten,
		/// <summary>
		/// Befehlstyp mit dem der Server die Position der Basis übergibt.
		/// </summary>
		basisBauen,
		/// <summary>
		/// Befehlstyp fürs bauen für Berge. Typ,X,Y
		/// </summary>
		bergBauen
	}


	/// <summary>
	/// Die Statische Klasse für Methoden die von Server und Client aufgerufen werden müssen und für Globale Methoden/Enumerationen
	/// </summary>
	public static class Global
	{
		private static Random rnd;
		/// <summary>
		/// Random Atribut
		/// </summary>
		public static Random Rnd
		{
			get { if (rnd == null) rnd = new Random(); return rnd; }
			set { }
		}
		/// <summary>
		/// Statisches attribut zur festlegung der spielfeldgröße
		/// </summary>
		public const int feldgroesse = 20;
		/// <summary>
		/// Statisches attribut welches die pixelgröße der blöcke festlegt (stimmt mit der bildergröße überein)
		/// </summary>
		public const int blockgroesse = 30;
		/// <summary>
		/// das geld mit dem jeder spieler in einer normalen spielrunde anfängt
		/// </summary>
		public static int startgeld = 100; //kann jederzeit geändert werden
		/// <summary>
		/// Die ernergie mit der jeder Dynamische turm anfängt.
		/// </summary>
		public static int startenergie = 1000;
		/// <summary>
		/// Die maximale schildmenge für einen großteil der türme
		/// </summary>
		public static int standartSchild = 200;

		/// <summary>
		/// Anfang des Button1s in Pixel
		/// </summary>
		public static Point button1Anfang = new Point(250, 640);
		/// <summary>
		/// Ende des Buttons1s in Pixel
		/// </summary>
		public static Point button1Ende = new Point(450, 680);
		/// <summary>
		/// Anfang des Button2s in Pixel
		/// </summary>
		public static Point button2Anfang = new Point(500, 640);
		/// <summary>
		/// Ende des Buttons2
		/// </summary>
		public static Point button2Ende = new Point(700, 680);
		/// <summary>
		/// Anfang des Button6 in Pixeln
		/// </summary>
		public static Point button6Anfang = new Point(750, 720);
		/// <summary>
		/// Ende des Button6 in Pixeln
		/// </summary>
		public static Point button6Ende = new Point(950, 800);

		/// <summary>
		/// Entschlüßelt eine String Nachricht für den Clienten und führt sie gleich aus 
		/// </summary>
		/// <param name="befehl"> Die zu entschlüßelnde String nachricht</param>
		/// <param name="client">Objektverweiß zu dem Clienten</param>
		public static void entschluesseln(string befehl, Kommunikation.Client client)
		{

			Befehlstyp befehlstyp = (Befehlstyp)befehl[0];
			switch (befehlstyp)
			{
				case Befehlstyp.verbessern:
					if ((Spieler)befehl[4] == client.form.spieler)
					{
						client.me.geld--;
					}
					client.form.verbesserung((Typ)befehl[3], (int)befehl[1], (int)befehl[2], (Spieler)befehl[4]);
					break;
				case Befehlstyp.verbessernStats:
					if ((Spieler)befehl[4] == client.form.spieler)
					{
						client.me.geld--;
					}
					client.form.verbesserungStats((Typ)befehl[3], (int)befehl[1], (int)befehl[2], (Spieler)befehl[4]);
					break;
				case Befehlstyp.turmDrehen:

					byte[] byteArray = new byte[8];
					for (int i = 3; i <= 10; i++)
					{
						byteArray[i - 3] = (byte)befehl[i];
					}
					float vektorX = BitConverter.ToSingle(byteArray, 0);
					float vektorY = BitConverter.ToSingle(byteArray, 4);

					if (((Spieler)befehl[11]) == client.form.spieler)
					{
						if (client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]] != null)
						{
							client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD.ausrichten(vektorX, vektorY);
						}
						client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD.feuerbefehl = true;
						break;
					}
					else
					{
						if (client.form.karteStatisch.karteS[feldgroesse * 2 - (int)befehl[1] - 1, (int)befehl[2]] != null)
						{
							client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])].turmD.ausrichten(vektorX * -1, vektorY);
						}
						client.form.karteStatisch.karteS[feldgroesse * 2 - (int)befehl[1] - 1, (int)befehl[2]].turmD.feuerbefehl = true;
						break;
					}
				case Befehlstyp.turmKaput:
					if (((Spieler)befehl[3]) == Spieler.spielerA)
					{
						if (((Spieler)befehl[3]) == client.form.spieler)
						{
							if (client.form.gebaudeauswahl == client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]]) client.form.gebaudeauswahl = null;
							if (client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD != null) client.form.karteDynamisch.karteTD.Remove(client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD); //deinitialisierung fals nötig
							client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]] = null;
							client.form.hintergrundAktuell = false;
							break;
						}
						if (client.form.gebaudeauswahl == client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])]) client.form.gebaudeauswahl = null;
						if (client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])].turmD != null) client.form.karteDynamisch.karteTD.Remove(client.form.karteStatisch.karteS[feldgroesse * 2 - (int)befehl[1] - 1, (int)befehl[2]].turmD); //deinitialisierung fals nötig
						client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])] = null;
						client.form.hintergrundAktuell = false;

						break;
					}
					else
					{
						if (((Spieler)befehl[3]) == client.form.spieler)
						{
							if (client.form.gebaudeauswahl == client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])]) client.form.gebaudeauswahl = null;
							if (client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])].turmD != null) client.form.karteDynamisch.karteTD.Remove(client.form.karteStatisch.karteS[feldgroesse * 2 - (int)befehl[1] - 1, (int)befehl[2]].turmD); //deinitialisierung fals nötig
							client.form.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])] = null;
							client.form.hintergrundAktuell = false;
							break;
						}
						if (client.form.gebaudeauswahl == client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]]) client.form.gebaudeauswahl = null;
						if (client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD != null) client.form.karteDynamisch.karteTD.Remove(client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD); //deinitialisierung fals nötig
						client.form.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]] = null;
						client.form.hintergrundAktuell = false;
						break;
					}
				case Befehlstyp.zielmodus:
					client.form.zielmodus = true;
					client.form.baumodus = false;
					client.form.hintergrundAktuell = false;
					break;
				case Befehlstyp.baumodus:
					client.form.spielLauft = true;
					client.form.baumodus = true;
					break;
				case Befehlstyp.kampfmodus:
					client.form.zielmodus = false;
					break;
				case Befehlstyp.ende:
					client.form.spielLauft = false;
					if ((Spieler)befehl[1] == client.form.spieler)
					{
						client.form.gewonnen = true;
					}
					else
					{
						client.form.verloren = true;
					}

					break;
				case Befehlstyp.bezierAusrichten:
					Point P1 = new Point((int)befehl[2], (int)befehl[3]);
					Point P2 = new Point((int)befehl[4], (int)befehl[5]);
					Point P3 = new Point((int)befehl[6], (int)befehl[7]);
					Point P4 = new Point((int)befehl[8], (int)befehl[9]);


					if (((Spieler)befehl[1]) == client.form.spieler)
					{
						if (client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30] != null)
						{
							client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P1 = P1;
							client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P2 = P2;
							client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P3 = P3;
							client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P4 = P4;
							client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.feuerbefehl = true;

							Point vektor = new Point(P2.X - P1.X, P2.Y - P1.Y);
							client.form.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.ausrichten((float)((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.X), (float)((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.Y));
						}

					}
					else
					{
						if (client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30] != null)
						{
							client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P1 = new Point(feldgroesse * 2 * blockgroesse - P1.X, P1.Y);
							client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P2 = new Point(feldgroesse * 2 * blockgroesse - P2.X, P2.Y);
							client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P3 = new Point(feldgroesse * 2 * blockgroesse - P3.X, P3.Y);
							client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P4 = new Point(feldgroesse * 2 * blockgroesse - P4.X, P4.Y);
							client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.feuerbefehl = true;

							Point vektor = new Point((feldgroesse * 2 - P2.X - 1) - (feldgroesse * 2 - P1.X - 1), P2.Y - P1.Y);
							client.form.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.ausrichten((float)((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.X), (float)((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.Y));
						}
					}
					break;
				case Befehlstyp.basisBauen:
					int x = (int)befehl[1];
					int y = (int)befehl[2];
					if (client.form.spieler == Spieler.spielerA)
					{
						client.form.karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Basis(new Point(x, y), Spieler.spielerA, 0, 0, 0);
						client.form.karteStatisch.karteS[x, y].initialisieren();
						client.form.karteStatisch.karteS[feldgroesse * 2 - x - 1, y] = new Statisch.Verbesserungen.Basis(new Point(feldgroesse * 2 - x - 1, y), Spieler.spielerB, 0, 0, 0);
						client.form.karteStatisch.karteS[feldgroesse * 2 - x - 1, y].initialisieren();
					}
					else
					{
						client.form.karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Basis(new Point(x, y), Spieler.spielerB, 0, 0, 0);
						client.form.karteStatisch.karteS[x, y].initialisieren();
						client.form.karteStatisch.karteS[feldgroesse * 2 - x - 1, y] = new Statisch.Verbesserungen.Basis(new Point(feldgroesse * 2 - x - 1, y), Spieler.spielerA, 0, 0, 0);
						client.form.karteStatisch.karteS[feldgroesse * 2 - x - 1, y].initialisieren();
					}
					client.form.hintergrundAktuell = false;
					break;
				case Befehlstyp.bergBauen:
					int x2 = (int)befehl[1];
					int y2 = (int)befehl[2];
					client.form.karteStatisch.karteS[x2, y2] = new Statisch.Verbesserungen.Berg(new Point(x2, y2), Spieler.umgebung);
					client.form.karteStatisch.karteS[x2, y2].initialisieren();
					client.form.karteStatisch.karteS[feldgroesse * 2 - x2 - 1, y2] = new Statisch.Verbesserungen.Berg(new Point(feldgroesse * 2 - x2 - 1, y2), Spieler.umgebung);
					client.form.karteStatisch.karteS[feldgroesse * 2 - x2 - 1, y2].initialisieren();
					break;
			}
		}

		/// <summary>
		/// Entschlüßelt eine String Nachricht für den Server und führt sie gleich aus
		/// </summary>
		/// <param name="befehl">Die zu entschlüßelnde String nachricht</param>
		/// <param name="server">Objektverweiß zu dem Server</param>
		public static void entschluesseln(string befehl, Kommunikation.Server server)
		{

			Befehlstyp befehlstyp = (Befehlstyp)befehl[0];
			switch (befehlstyp)
			{
				case Befehlstyp.verbessern:
					if (server.voll && server.con_list[0].spieler == (Spieler)befehl[4] && server.con_list[0].geld > 0 && server.console.baumodus)
					{
						if (server.console.verbesserung((Typ)befehl[3], (int)befehl[1], (int)befehl[2], (Spieler)befehl[4]))
						{
							server.con_list[0].geld--;
							server.schreiben(befehl);
						}
						break;
					}
					else if (server.voll && server.con_list[1].spieler == (Spieler)befehl[4] && server.con_list[1].geld > 0 && server.console.baumodus)
					{
						if (server.console.verbesserung((Typ)befehl[3], (int)befehl[1], (int)befehl[2], (Spieler)befehl[4]))
						{
							server.con_list[1].geld--;
							server.schreiben(befehl);
						}
						break;
					}
					else
					{
						break;
					}
				case Befehlstyp.verbessernStats:
					if (server.voll && server.con_list[0].spieler == (Spieler)befehl[4] && server.con_list[0].geld > 0 && server.console.baumodus)
					{
						if (server.console.verbesserungStats((Typ)befehl[3], (int)befehl[1], (int)befehl[2], (Spieler)befehl[4]))
						{
							server.con_list[0].geld--;
							server.schreiben(befehl);
						}
						break;
					}
					else if (server.voll && server.con_list[1].spieler == (Spieler)befehl[4] && server.con_list[1].geld > 0 && server.console.baumodus)
					{
						if (server.console.verbesserungStats((Typ)befehl[3], (int)befehl[1], (int)befehl[2], (Spieler)befehl[4]))
						{
							server.con_list[1].geld--;
							server.schreiben(befehl);
						}
						break;
					}
					else
					{
						break;
					}
				case Befehlstyp.turmDrehen:
					byte[] byteArray = new byte[8];
					for (int i = 3; i <= 10; i++)
					{
						byteArray[i - 3] = (byte)befehl[i];
					}
					float vektorX = BitConverter.ToSingle(byteArray, 0);
					float vektorY = BitConverter.ToSingle(byteArray, 4);

					if (((Spieler)befehl[11]) == Spieler.spielerA)
					{
						if (server.console.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]] != null)
						{
							server.console.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD.ausrichten(vektorX, vektorY);
							server.console.karteStatisch.karteS[(int)befehl[1], (int)befehl[2]].turmD.feuerbefehl = true;
							server.schreiben(befehl);
						}

					}
					else
					{
						if (server.console.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, (int)befehl[2]] != null)
						{
							server.console.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])].turmD.ausrichten(vektorX * -1, vektorY);
							server.console.karteStatisch.karteS[feldgroesse * 2 - ((int)befehl[1]) - 1, ((int)befehl[2])].turmD.feuerbefehl = true;
							server.schreiben(befehl);
						}
					}
					break;
				case Befehlstyp.bezierAusrichten:
					Point P1 = new Point((int)befehl[2], (int)befehl[3]);
					Point P2 = new Point((int)befehl[4], (int)befehl[5]);
					Point P3 = new Point((int)befehl[6], (int)befehl[7]);
					Point P4 = new Point((int)befehl[8], (int)befehl[9]);
					if (((Spieler)befehl[1]) == Spieler.spielerA)
					{
						if (server.console.karteStatisch.karteS[P1.X / 30, P1.Y / 30] != null)
						{
							server.console.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P1 = P1;
							server.console.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P2 = P2;
							server.console.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P3 = P3;
							server.console.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.P4 = P4;
							server.console.karteStatisch.karteS[P1.X / 30, P1.Y / 30].turmD.feuerbefehl = true;

							server.schreiben(befehl);
						}

					}
					else
					{
						if (server.console.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30] != null)
						{
							server.console.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P1 = new Point(feldgroesse * 2 * blockgroesse - P1.X, P1.Y);
							server.console.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P2 = new Point(feldgroesse * 2 * blockgroesse - P2.X, P2.Y);
							server.console.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P3 = new Point(feldgroesse * 2 * blockgroesse - P3.X, P3.Y);
							server.console.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.P4 = new Point(feldgroesse * 2 * blockgroesse - P4.X, P4.Y);
							server.console.karteStatisch.karteS[feldgroesse * 2 - P1.X / 30 - 1, P1.Y / 30].turmD.feuerbefehl = true;

							server.schreiben(befehl);
						}
					}
					break;
				default:
					break;//DIe Befehle die der Server niemals erhalten wird da es meist Statusmeldungen über das Spiel sind, welche der Client benötigt um synkron zu bleiben.
			}
		}

		/// <summary>
		/// Wird aufgerufen wenn Bilder rotiert werden sollen(nicht gespiegelt). (bei Projektile und Dynamische Türme)
		/// </summary>
		/// <param name="zuDrehendeBild">Das zu drehende Bild</param>
		/// <param name="rotationsWinkel">Der Rotationswinkel</param>
		/// <returns>Das Fertig gedrehte Bild</returns>
		public static Image RotateImage(Image zuDrehendeBild, float rotationsWinkel)
		{
			//Erstellt das leere Bild welches später fertig rotiert sein wird und zurückgegeben wird.
			Bitmap fertigeBild = new Bitmap(zuDrehendeBild.Width, zuDrehendeBild.Height);

			//Erstellt ein Graphics objekt von dem "fertigenBild"
			Graphics g = Graphics.FromImage(fertigeBild);

			//Rotationspunkt in die Mitte setzen
			g.TranslateTransform(((float)fertigeBild.Width / 2) - (float)0.5, ((float)fertigeBild.Height / 2) - (float)0.5);

			//Dreht das Bild
			g.RotateTransform(rotationsWinkel);//exception nötig? fehler

			g.TranslateTransform((float)0.5 - ((float)fertigeBild.Width / 2), (float)0.5 - ((float)fertigeBild.Height / 2));

			//set the InterpolationMode to HighQualityBicubic so to ensure a high
			//quality image once it is transformed to the specified size
			g.InterpolationMode = InterpolationMode.HighQualityBicubic;

			//Zeichnet g in das Bild
			g.DrawImage(zuDrehendeBild, new Point(0, 0));
			g.Dispose();

			return fertigeBild;
		}
	}
}
