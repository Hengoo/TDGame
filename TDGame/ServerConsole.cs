using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace TDGame
{
	/// <summary>
	/// Der Teil des Servers der die brechnung und verwalltung des Spiels übernimmt
	/// </summary>
	public class ServerConsole
	{
		/// <summary>
		/// True wenn der baumodus an ist
		/// </summary>
		public bool baumodus;
		/// <summary>
		/// Zeigt an ob der zielmodus aktiv ist.(kein kämpfen aber bereits ausrichten)
		/// </summary>
		public bool zielmodus;
		/// <summary>
		/// zeigt an ob das Spiel momentan lauft und alle spieler verbunden sind.
		/// </summary>
		public bool spielLauft;
		/// <summary>
		/// Objekt der karteStatisch
		/// </summary>
		public Statisch.KarteS karteStatisch;
		/// <summary>
		/// Objekt der KarteDynamisch
		/// </summary>
		public Dynamisch.KarteD karteDynamisch;
		/// <summary>
		/// Zählt jeden berechnungsschritt mit und wird bie 60 wieder auf 0 resetet.
		/// </summary>
		public int zaehler;
		/// <summary>
		/// Objekt zum zeitmessen um genaue 60 FPS zu erreichen
		/// </summary>
		Stopwatch sw = new Stopwatch();
		/// <summary>
		/// Counter für die FPS Berechnung
		/// </summary>
		int cnt = 0;
		/// <summary>
		/// Objektreferenz zu dem Teil des Servers der für die Kommunikation zuständig ist.
		/// </summary>
		private Kommunikation.Server server;
		/// <summary>
		/// Thread für Konstante 60 FPS sorgt, und deshalb alle 16 Millisekunden die physikalische brechnung startet.
		/// </summary>
		private Thread thr;
		/// <summary>
		/// Gibt die Koordinaten der Basis an
		/// </summary>
		public Point koordinatenBasis;

		/// <summary>
		/// Erstellt und initialisiert den Server und seine Attribute/Objekte
		/// </summary>
		public ServerConsole()
		{
			karteStatisch = new Statisch.KarteS();
			karteDynamisch = new Dynamisch.KarteD();
			baumodus = true;
			//InitializeComponent();

			zaehler = 0;

			koordinatenBasis = new Point(Global.Rnd.Next(Global.feldgroesse), Global.Rnd.Next(Global.feldgroesse));

			server = new Kommunikation.Server(this);

			thr = new Thread(cb);
			thr.Start();
			sw.Start();
		}

		/// <summary>
		/// Thread für Konstante 60 FPS sorgt, und deshalb alle 16 Millisekunden die physikalische brechnung startet.
		/// </summary>
		private void cb()
		{
			//Console.WriteLine(server.udpm.port);
			int zeit = 0;

			while (server.running)
			{
				if (sw.ElapsedMilliseconds <= cnt)
				{
					Thread.Sleep(1);
					continue;
				}
				cnt += 16;
				if (!server.voll)
				{
					Thread.Sleep(1);
					continue;
				}
				if ((!baumodus) && (!zielmodus)) updater();
				if (zaehler >= 60)
				{
					if (zeit == 0)
					{
						StringBuilder strb = new StringBuilder();
						baumodus = true;
						strb.Append((char)(Befehlstyp.baumodus));
						server.schreiben(strb.ToString());

						//Basis bauen und an Clients übertragen

						for (int i = 0; i < Global.feldgroesse; i++) //i steht für x
						{
							for (int j = 0; j < Global.feldgroesse; j++)//j steht für y
							{
								if (i != 0 && j != 0 && (karteStatisch.karteS[i, j - 1] != null))
								{
									if (Global.Rnd.Next(100) < 30)
									{
										StringBuilder strb3 = new StringBuilder();

										karteStatisch.karteS[i, j] = new Statisch.Verbesserungen.Berg(new Point(i, j), Spieler.umgebung);
										karteStatisch.karteS[i, j].initialisieren();
										karteStatisch.karteS[Global.feldgroesse * 2 - 1 - i, j] = new Statisch.Verbesserungen.Berg(new Point(Global.feldgroesse * 2 - 1 - i, j), Spieler.umgebung);
										karteStatisch.karteS[Global.feldgroesse * 2 - 1 - i, j].initialisieren();

										strb3.Append((char)(Befehlstyp.bergBauen));
										strb3.Append((char)(i));
										strb3.Append((char)(j));
										server.schreiben(strb3.ToString());
									}
								}
								else if (j < 4 && Global.Rnd.Next(100) < 20)
								{
									StringBuilder strb3 = new StringBuilder();

									karteStatisch.karteS[i, j] = new Statisch.Verbesserungen.Berg(new Point(i, j), Spieler.umgebung);
									karteStatisch.karteS[i, j].initialisieren();
									karteStatisch.karteS[Global.feldgroesse * 2 - 1 - i, j] = new Statisch.Verbesserungen.Berg(new Point(Global.feldgroesse * 2 - 1 - i, j), Spieler.umgebung);
									karteStatisch.karteS[Global.feldgroesse * 2 - 1 - i, j].initialisieren();

									strb3.Append((char)(Befehlstyp.bergBauen));
									strb3.Append((char)(i));
									strb3.Append((char)(j));
									server.schreiben(strb3.ToString());
								}
								else if (Global.Rnd.Next(100) < 10)
								{
									StringBuilder strb3 = new StringBuilder();

									karteStatisch.karteS[i, j] = new Statisch.Verbesserungen.Berg(new Point(i, j), Spieler.umgebung);
									karteStatisch.karteS[i, j].initialisieren();
									karteStatisch.karteS[Global.feldgroesse * 2 - 1 - i, j] = new Statisch.Verbesserungen.Berg(new Point(Global.feldgroesse * 2 - 1 - i, j), Spieler.umgebung);
									karteStatisch.karteS[Global.feldgroesse * 2 - 1 - i, j].initialisieren();

									strb3.Append((char)(Befehlstyp.bergBauen));
									strb3.Append((char)(i));
									strb3.Append((char)(j));
									server.schreiben(strb3.ToString());
								}
							}
						}


						StringBuilder strb2 = new StringBuilder();
						karteStatisch.karteS[koordinatenBasis.X, koordinatenBasis.Y] = new Statisch.Verbesserungen.Basis(koordinatenBasis, Spieler.spielerA, 0, 0, 0);
						karteStatisch.karteS[koordinatenBasis.X, koordinatenBasis.Y].initialisieren();
						karteStatisch.karteS[Global.feldgroesse * 2 - 1 - koordinatenBasis.X, koordinatenBasis.Y] = new Statisch.Verbesserungen.Basis(new Point(Global.feldgroesse * 2 - 1 - koordinatenBasis.X, koordinatenBasis.Y), Spieler.spielerB, 0, 0, 0);
						karteStatisch.karteS[Global.feldgroesse * 2 - 1 - koordinatenBasis.X, koordinatenBasis.Y].initialisieren();

						strb2.Append((char)(Befehlstyp.basisBauen));
						strb2.Append((char)(koordinatenBasis.X));
						strb2.Append((char)(koordinatenBasis.Y));
						server.schreiben(strb2.ToString());
					}

					if (zeit == 40)  //Dies und die zahl weiter unten ändern damit der Kampf früher anfängt. Bin leider nicht mehr dazu gekommen einen Bereit Button zu implementieren
					{
						StringBuilder strb = new StringBuilder();
						baumodus = false;
						zielmodus = true;
						strb.Append((char)(Befehlstyp.zielmodus));
						server.schreiben(strb.ToString());
					}
					if (zeit == 42)//diese zahl hier
					{
						StringBuilder strb = new StringBuilder();
						zielmodus = false;
						strb.Append((char)(Befehlstyp.kampfmodus));
						server.schreiben(strb.ToString());
					}
					zaehler = 0;
					Console.Write("{0} ", zeit);
					zeit++;

				}
				zaehler++;
			}
		}

		/// <summary>
		/// Geht alle dynamischen objekte durch und berechnet den nächsten Berechnungsschritt
		/// </summary>
		public void updater()
		{
			for (int i = karteDynamisch.karteKD.Count - 1; i >= 0; i--)
			{
				Dynamisch.Kugel kugel = karteDynamisch.karteKD[i];
				if (kugel.naechsteKoordinaten().X < 1200 && kugel.naechsteKoordinaten().X > 0 && kugel.naechsteKoordinaten().Y > 0 && kugel.naechsteKoordinaten().Y < 600)//kugel auserhalt her überprüfen
				{
					if (karteStatisch.testeKollision(kugel.bewegen(), kugel.ursprungskoordinaten, kugel.spieler, kugel.projektil))
					{
						int xKugel = ((int)kugel.koordinaten.X) / Global.blockgroesse;
						int yKugel = ((int)kugel.koordinaten.Y) / Global.blockgroesse;
						if (karteStatisch.karteS[xKugel, yKugel].schadenBekommen(kugel.schadenmachen(), kugel.projektil))//kugel macht momentan schadne und töten , SERVER halt
						{
							if (karteStatisch.karteS[xKugel, yKugel].turmD != null) karteDynamisch.karteTD.Remove(karteStatisch.karteS[xKugel, yKugel].turmD);//löschen des dynamischen objekts fals vorhanden
							StringBuilder strb = new StringBuilder();
							strb.Append((char)(Befehlstyp.turmKaput));
							strb.Append((char)(xKugel));
							strb.Append((char)(yKugel));
							strb.Append((char)(karteStatisch.karteS[xKugel, yKugel].spieler));
							server.schreiben(strb.ToString());
							karteStatisch.karteS[xKugel, yKugel] = null; //Turm wird hier zertört
						}
						karteDynamisch.karteKD.RemoveAt(i);
					}
				}
				else karteDynamisch.karteKD.RemoveAt(i);
			}

			for (int i = 0; i < karteDynamisch.karteTD.Count; i++)
			{
				if (karteDynamisch.karteTD[i].angreifen()) karteDynamisch.karteKD.Add(karteDynamisch.karteTD[i].feuern());
			}

			//überprüfung ob das spiel bereits vorbei ist.(eine basis zerstört wurde)
			if (karteStatisch.karteS[koordinatenBasis.X, koordinatenBasis.Y] == null)
			{
				StringBuilder strb = new StringBuilder();
				strb.Append((char)(Befehlstyp.ende));
				strb.Append((char)(Spieler.spielerB));
				server.schreiben(strb.ToString());
			}
			else if (karteStatisch.karteS[Global.feldgroesse * 2 - koordinatenBasis.X - 1, koordinatenBasis.Y] == null)
			{
				StringBuilder strb = new StringBuilder();
				strb.Append((char)(Befehlstyp.ende));
				strb.Append((char)(Spieler.spielerA));
				server.schreiben(strb.ToString());
			}
		}

		/// <summary>
		/// Verbessert einen Turm
		/// </summary>
		/// <param name="typ">Der Verbesserungstyp</param>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spielerauswahl">Der Spieler dem der Block gehört/Gehören soll</param>
		/// <returns>Gibt zurück ob das Gebäude verbessert werden konnte, false = nein</returns>
		public bool verbesserung(Typ typ, int x, int y, Spieler spielerauswahl)
		{
			if (spielerauswahl == Spieler.spielerA)
			{
				switch (typ)
				{
					case Typ.wall: return verbesserungWall(x, y, spielerauswahl);
					case Typ.turm: return verbesserungTurm(x, y, spielerauswahl);
					case Typ.spezial: return verbesserungSpezial(x, y, spielerauswahl);
				}
			}
			else
			{
				switch (typ)
				{
					case Typ.wall: return verbesserungWall(Global.feldgroesse * 2 - x - 1, y, spielerauswahl);
					case Typ.turm: return verbesserungTurm(Global.feldgroesse * 2 - x - 1, y, spielerauswahl);
					case Typ.spezial: return verbesserungSpezial(Global.feldgroesse * 2 - x - 1, y, spielerauswahl);
				}
			}
			return false;
		}

		/// <summary>
		/// Verbessert einen Block mit dem Wallupgrade oder erschafft einen Wall
		/// </summary>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spieleraktuell">Der Spieler dem der Block gehört/Gehören soll</param>
		/// <returns> gibt zurück ob das gebaude verbessert werden konnte, False = nein</returns>
		public bool verbesserungWall(int x, int y, Spieler spieleraktuell)
		{
			if (karteStatisch.karteS[x, y] != null)
			{
				Statisch.Block block = karteStatisch.karteS[x, y].verbessern(Typ.wall);
				if (block != null)
				{
					if (karteStatisch.karteS[x, y].turmD != null)
					{
						karteDynamisch.karteTD.Remove(karteStatisch.karteS[x, y].turmD);//löschen des dynamischen objekts fals vorhanden
						karteStatisch.karteS[x, y] = block; //überschreiben von dem alten turm mit dem neuen

						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
					}
					else
					{
						karteStatisch.karteS[x, y] = block;//überschreiben des alten Turms mit dem neuen
						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Wall(new Point(x, y), spieleraktuell, 0, 0, 0);//erzeugen von objekt

				Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
				if (turmD != null) karteDynamisch.karteTD.Add(turmD);
			}
			return true;
		}

		/// <summary>
		/// Verbessert einen Block mit dem Turmupgrade oder erschafft einen Wall
		/// </summary>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spieleraktuell">Der Spieler dem der Block gehört/Gehören soll</param>
		/// <returns> gibt zurück ob das gebaude verbessert werden konnte, False = nein</returns>
		public bool verbesserungTurm(int x, int y, Spieler spieleraktuell)
		{
			if (karteStatisch.karteS[x, y] != null)
			{
				Statisch.Block block = karteStatisch.karteS[x, y].verbessern(Typ.turm);
				if (block != null)
				{
					if (karteStatisch.karteS[x, y].turmD != null)
					{
						karteDynamisch.karteTD.Remove(karteStatisch.karteS[x, y].turmD);//löschen des dynamischen objekts fals vorhanden
						karteStatisch.karteS[x, y] = block; //überschreiben von dem alten turm mit dem neuen

						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
					}
					else
					{
						karteStatisch.karteS[x, y] = block;//überschreiben des alten Turms mit dem neuen
						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Turm(new Point(x, y), spieleraktuell, 0, 0, 0);//erzeugen von objekt

				Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
				if (turmD != null) karteDynamisch.karteTD.Add(turmD);
			}
			return true;
		}

		/// <summary>
		/// Verbessert einen Block mit dem Spezialupgrade oder erschafft einen Wall
		/// </summary>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spieleraktuell">Der Spieler dem der Block gehört/Gehören soll</param>
		/// <returns> gibt zurück ob das gebaude verbessert werden konnte, False = nein</returns>
		public bool verbesserungSpezial(int x, int y, Spieler spieleraktuell)
		{
			if (karteStatisch.karteS[x, y] != null)
			{
				Statisch.Block block = karteStatisch.karteS[x, y].verbessern(Typ.spezial);
				if (block != null)
				{
					if (karteStatisch.karteS[x, y].turmD != null)
					{
						karteDynamisch.karteTD.Remove(karteStatisch.karteS[x, y].turmD);//löschen des dynamischen objekts fals vorhanden
						karteStatisch.karteS[x, y] = block; //überschreiben von dem alten turm mit dem neuen

						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
					}
					else
					{
						karteStatisch.karteS[x, y] = block;//überschreiben des alten Turms mit dem neuen
						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
					}
				}
				else
				{
					return false;
				}
			}
			else
			{
				karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Spezial(new Point(x, y), spieleraktuell, 0, 0, 0);//erzeugen von objekt

				Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
				if (turmD != null) karteDynamisch.karteTD.Add(turmD);
			}
			return true;
		}

		/// <summary>
		/// Wird Beim schließen des Servers aufgerufen, und beendet alle laufenden Threads
		/// </summary>
		public void Close()
		{
			server.schliessen();
			thr.Abort();
		}

		/// <summary>
		/// Gibt einem Block eine Statverbesserung
		/// </summary>
		/// <param name="typ">Verbesserungs Typ</param>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		/// <param name="spielerauswahl">Der Spieler der die Verbesserung durchführt</param>
		/// <returns>Gibt an ob die Verbesserung möglich ist</returns>
		public bool verbesserungStats(Typ typ, int x, int y, Spieler spielerauswahl) //das und alles darunter ist ?? richtig, ?spieler? ?? brauch ich die methiden überhauptnoch?
		{
			if (spielerauswahl == Spieler.spielerA)
			{
				switch (typ)
				{
					case Typ.wall: return verbesserungStatsWall(x, y);
					case Typ.turm: return verbesserungStatsTurm(x, y);
					case Typ.spezial: return verbesserungStatsSpezial(x, y);
				}
			}
			else
			{
				switch (typ)
				{
					case Typ.wall: return verbesserungStatsWall(Global.feldgroesse * 2 - x - 1, y);
					case Typ.turm: return verbesserungStatsTurm(Global.feldgroesse * 2 - x - 1, y);
					case Typ.spezial: return verbesserungStatsSpezial(Global.feldgroesse * 2 - x - 1, y);
				}
			}
			return false;
		}

		/// <summary>
		/// Verbessert den Deffensiven Stat eines Blockes
		/// </summary>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		/// <returns>Gibt an ob die Verbesserung möglich ist</returns>
		public bool verbesserungStatsWall(int x, int y)
		{
			if (karteStatisch.karteS[x, y] != null) return karteStatisch.karteS[x, y].verbessernstats(Typ.wall);
			return false;
		}

		/// <summary>
		/// Verbessert den Offensive Stat eines Blockes
		/// </summary>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		/// <returns>Gibt an ob die Verbesserung möglich ist</returns>
		public bool verbesserungStatsTurm(int x, int y)
		{
			if (karteStatisch.karteS[x, y] != null) return karteStatisch.karteS[x, y].verbessernstats(Typ.turm);
			return false;
		}

		/// <summary>
		/// Verbessert den Speziellen Stat eines Blockes
		/// </summary>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		/// <returns>Gibt an ob die Verbesserung möglich ist</returns>
		public bool verbesserungStatsSpezial(int x, int y)
		{
			if (karteStatisch.karteS[x, y] != null) return karteStatisch.karteS[x, y].verbessernstats(Typ.spezial);
			return false;
		}

		/// <summary>
		/// Resetet den Server damit eine Neue Patie gestartet werden kann.
		/// </summary>
		public void Reset()
		{
			server.schliessen();
			thr.Abort();

			karteStatisch = new Statisch.KarteS();
			karteDynamisch = new Dynamisch.KarteD();
			baumodus = true;
			zielmodus = false;
			spielLauft = false;

			server = new Kommunikation.Server(this);
			zaehler = 0;

			koordinatenBasis = new Point(Global.Rnd.Next(Global.feldgroesse), Global.Rnd.Next(Global.feldgroesse));

			thr = new Thread(cb);
			thr.Start();
			sw.Start();
		}
	}
}
