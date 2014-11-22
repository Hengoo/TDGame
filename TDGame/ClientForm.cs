using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Diagnostics;


namespace TDGame
{
	/// <summary>
	/// Der Client der alles verwaltet und brechnet
	/// </summary>
	public partial class ClientForm : Form
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
		/// True wenn man gewonnen hat
		/// </summary>
		public bool gewonnen;
		/// <summary>
		/// True wenn man verloren hat
		/// </summary>
		public bool verloren;
		/// <summary>
		/// wird true wenn das Spiel aufgrund verbindungsabrissen beendet wird.
		/// </summary>
		public bool verbindungGetrennt;
		/// <summary>
		/// Objekt der karteStatisch
		/// </summary>
		public Statisch.KarteS karteStatisch;
		/// <summary>
		/// Objekt der KarteDynamisch
		/// </summary>
		public Dynamisch.KarteD karteDynamisch;
		/// <summary>
		/// Objektreferenz zu dem mit der maus ausgewälten gebaudes
		/// </summary>
		public Statisch.Block gebaudeauswahl;
		/// <summary>
		/// Koordinaten von ausgewählten feldern die noch kein gebaude haben
		/// </summary>
		public Point auswahl;
		/// <summary>
		/// Wenn das hintergrundbild nicht mehr aktuell ist wird hintergrundAktuell false.
		/// </summary>
		public bool hintergrundAktuell;
		/// <summary>
		/// Das Bild der Statischen Objekte das gespeichert wird damit es nicht bei jedem zeichnen neu berechnet werden muss.
		/// </summary>
		public Bitmap hintergrund;
		/// <summary>
		/// Das gespiegelte Gras Bild
		/// </summary>
		public Bitmap grassFlip;
		/// <summary>
		/// Das Gras Bild
		/// </summary>
		private Bitmap grass;
		/// <summary>
		/// Objektreferenz zu dem Cliententeil der für die Kommunikation verwantwortlich ist
		/// </summary>
		private Kommunikation.Client client;
		/// <summary>
		/// Speichert zu welchem Spieler der Client gehört
		/// </summary>
		public Spieler spieler;
		/// <summary>
		/// Zeigt an ob Gerade eine Drag and Drop aktion aktiv ist
		/// </summary>
		public bool dragDrop;
		/// <summary>
		/// Punkt für die Ausrichtung von BezierTürmen
		/// </summary>
		public Point bezierPunkt1;
		/// <summary>
		/// Punkt für die Ausrichtung von BezierTürmen
		/// </summary>
		public Point bezierPunkt2;
		/// <summary>
		/// Punkt für die Ausrichtung von BezierTürmen
		/// </summary>
		public Point bezierPunkt3;
		/// <summary>
		/// Punkt für die Ausrichtung von BezierTürmen
		/// </summary>
		public bool bezierDragDrop;
		/// <summary>
		/// Objekt zum zeitmessen um genaue 60 FPS zu erreichen
		/// </summary>
		Stopwatch sw = new Stopwatch();
		/// <summary>
		/// Counter für die FPS Berechnung
		/// </summary>
		int cnt = 0;
		/// <summary>
		/// Gibt an ob der Client noch lauft, wenn es false wird werden die Threads geschloßen
		/// </summary>
		public bool running = true;
		/// <summary>
		/// Thread für Konstante 60 FPS sorgt, Und deshalb alle 16 Millisekunden die physikalische brechnung startet und das bild danach zeichnet.
		/// </summary>
		private Thread thr;
		/// <summary>
		/// Wird zur Vorschau des Walls benötigt
		/// </summary>
		private Statisch.Block vorschau1;
		/// <summary>
		/// wird zur Vorschau des Turms benötigt
		/// </summary>
		private Statisch.Block vorschau2;
		/// <summary>
		/// wird zur Vorschau des SpezialTurms benötigt
		/// </summary>
		private Statisch.Block vorschau3;

		/// <summary>
		/// Erstellt und Initialisiert den Clienten und seine Attribute
		/// </summary>
		public ClientForm()
		{
			karteStatisch = new Statisch.KarteS();
			karteDynamisch = new Dynamisch.KarteD();

			baumodus = false;
			zielmodus = false;
			spielLauft = false;
			dragDrop = false;
			bezierDragDrop = false;
			gewonnen = false;
			verloren = false;
			verbindungGetrennt = false;

			vorschau1 = new Statisch.Verbesserungen.Wall(new Point(0, 0), spieler, 0, 0, 0);
			vorschau2 = new Statisch.Verbesserungen.Turm(new Point(0, 0), spieler, 0, 0, 0);
			vorschau3 = new Statisch.Verbesserungen.Spezial(new Point(0, 0), spieler, 0, 0, 0);
			if (vorschau1 != null)
			{
				vorschau1.initialisieren();
			}
			if (vorschau2 != null)
			{
				vorschau2.initialisieren();
			}
			if (vorschau3 != null)
			{
				vorschau3.initialisieren();
			}

			auswahl.X = 1000;
			bezierPunkt3.X = 10000;

			hintergrundAktuell = false;
			hintergrund = new Bitmap(1200, 600);
			grass = Properties.Resources.Grass10;
			grassFlip = Properties.Resources.Grass10;
			grassFlip.RotateFlip(RotateFlipType.RotateNoneFlipX);

			client = new Kommunikation.Client(this);
			client.me.geld = Global.startgeld;

			InitializeComponent();

			thr = new Thread(cb);
			thr.Start();
			sw.Start();
		}

		/// <summary>
		/// Thread für Konstante 60 FPS sorgt, Und deshalb alle 16 Millisekunden die physikalische brechnung startet und das bild danach zeichnenen lässt.
		/// </summary>
		private void cb()
		{
			while (running)
			{
				if (sw.ElapsedMilliseconds > cnt)
				{
					cnt += 16;
					if ((!baumodus) && (!zielmodus)) updater();
					zeichenfenster.Invalidate();
				}
				else
				{
					Thread.Sleep(1);
				}
			}
		}

		/*Außkommentierter Text wird zum zeichnen einer BezierKurve mit Hilfslinien Verwendet.
		private Point vektorerrechnen(Point PAnfang, Point PEnde)
		{
			return new Point(PEnde.X - PAnfang.X, PEnde.Y - PAnfang.Y);
		}
		private Point punkterrechnen(Point PAnfang, Point PEnde, double tberechnen)
		{
			Point vektor = vektorerrechnen(PAnfang, PEnde);
			return new Point((int)(vektor.X * tberechnen + PAnfang.X), (int)(vektor.Y * tberechnen + PAnfang.Y));
		}*/


		/// <summary>
		/// Zeichnet den Inhalt des Zeichenfensters
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void zeichenfenster_Paint(object sender, PaintEventArgs e)
		{
			/*
			 * Außkommentierter Text zeichnet eine Große Bezierkurve mit allen Hilfslinien. Damit wurde die Graphik in der Seminararbeit gezeichnet.
			Graphics g = e.Graphics;
			Point P1 = new Point(100,500);
			Point P2 = new Point(350,200);
			Point P3 = new Point(1100,50);
			Point P4 = new Point(1100,550);
			 * 
			Point P5 = punkterrechnen(P1, P2, 0.6);
			Point P6 = punkterrechnen(P2, P3, 0.6);
			Point P7 = punkterrechnen(P3, P4, 0.6);
			Point P8 = punkterrechnen(P5, P6, 0.6);
			Point P9 = punkterrechnen(P6, P7, 0.6);
			Point P10 = punkterrechnen(P8, P9, 0.6);
			 * 
			g.DrawLine(new Pen(Brushes.Black,3), P1, P2);
			g.DrawLine(new Pen(Brushes.Black, 3), P3, P4);
			g.DrawLine(new Pen(Brushes.Black, 3), P2, P3);
			g.DrawLine(new Pen(Brushes.Gray, 3), P5, P6);
			g.DrawLine(new Pen(Brushes.Gray, 3), P6, P7);
			g.DrawLine(new Pen(Brushes.DarkGray, 3), P8, P9);
			g.DrawBezier(new Pen(Brushes.Red, 3), P1, P2, P3, P4);
			g.DrawLine(Pens.Green, new Point(P10.X, P10.Y - 2), P10);
			 */
			Graphics g = e.Graphics;

			//Überprüft ob das alte Hintergrundbild(statische Blöcke und Türme) aktuell ist, wenn ja zeichnet er das, ansonsten updated er es und zeichnet es danach.
			if (!hintergrundAktuell)
			{
				Graphics meineBitMap = Graphics.FromImage(hintergrund);
				for (int i = 0; i < Global.feldgroesse * 2; i++)
				{
					for (int j = 0; j < Global.feldgroesse; j++)
					{
						if (karteStatisch.karteS[i, j] != null)
						{
							if (!baumodus) karteStatisch.karteS[i, j].zeichnen(meineBitMap);
							else
							{
								if (i < Global.feldgroesse) karteStatisch.karteS[i, j].zeichnenBaumodus(meineBitMap);
							}
						}
						else if (i < Global.feldgroesse) meineBitMap.DrawImage(grass, i * Global.blockgroesse, j * Global.blockgroesse, Global.blockgroesse, Global.blockgroesse);
						else meineBitMap.DrawImage(grassFlip, i * Global.blockgroesse, j * Global.blockgroesse, Global.blockgroesse, Global.blockgroesse);
					}
				}
				hintergrundAktuell = true;
			}
			g.DrawImage(hintergrund, 0, 0);


			//dynamische Türme mit drehrichtung zeichnen
			for (int i = 0; i < karteDynamisch.karteTD.Count; i++)
			{
				if (baumodus) karteDynamisch.karteTD[i].zeichnen(g, spieler);
				else karteDynamisch.karteTD[i].zeichnen(g);
			}


			//Hud mit beschriftung zeichnen
			g.DrawImage(Properties.Resources.HudHintergrund, 0, 600, 1200, 200);

			if (spielLauft)
			{
				g.DrawString("Geld: " + client.me.geld, SystemFonts.DefaultFont, Brushes.White, 30, 630);
				if (baumodus) g.DrawString("Baumodus", SystemFonts.DefaultFont, Brushes.White, 30, 670);
				if (zielmodus) g.DrawString("Zielmodus", SystemFonts.DefaultFont, Brushes.White, 30, 670);
				if ((!baumodus) && (!zielmodus)) g.DrawString("Kampfmodus", SystemFonts.DefaultFont, Brushes.White, 30, 670);
			}
			else
			{
				if (gewonnen) g.DrawString("Gewonnen", SystemFonts.CaptionFont, Brushes.White, 300, 300);
				else if (verloren) g.DrawString("Verloren", SystemFonts.CaptionFont, Brushes.White, 300, 300);
				else if (verbindungGetrennt) g.DrawString("Verbindung Unterbrochen", SystemFonts.CaptionFont, Brushes.White, 300, 300);
				else g.DrawString("Warte auf Spieler", SystemFonts.DefaultFont, Brushes.White, 30, 670);
			}

			if (gebaudeauswahl != null)
			{
				g.DrawString("HP: " + gebaudeauswahl.hP, SystemFonts.DefaultFont, Brushes.White, 1030, 630);
				g.DrawString("Schild: " + gebaudeauswahl.schild, SystemFonts.DefaultFont, Brushes.White, 1030, 670);
				if (gebaudeauswahl.turmD != null)
				{
					g.DrawString("Schaden: " + gebaudeauswahl.turmD.schaden, SystemFonts.DefaultFont, Brushes.White, 1030, 710);
					g.DrawString("Energie: " + gebaudeauswahl.turmD.energie, SystemFonts.DefaultFont, Brushes.White, 1030, 750);
				}
				else
				{
					g.DrawString("Rüstung: " + gebaudeauswahl.ruestung, SystemFonts.DefaultFont, Brushes.White, 1030, 710);
				}
			}

			//buttons zeichnen
			g.DrawRectangle(Pens.White, Global.button1Anfang.X, Global.button1Anfang.Y, 200, 40);
			g.DrawString("Wallupgrade", SystemFonts.DefaultFont, Brushes.White, Global.button1Anfang);

			g.DrawRectangle(Pens.White, Global.button2Anfang.X, Global.button1Anfang.Y, 200, 40);
			g.DrawString("Turmupgrade", SystemFonts.DefaultFont, Brushes.White, Global.button2Anfang);

			g.DrawRectangle(Pens.White, Global.button6Anfang.X, Global.button1Anfang.Y, 200, 40);
			g.DrawString("Spezialupgrade", SystemFonts.DefaultFont, Brushes.White, Global.button6Anfang.X, Global.button1Anfang.Y);

			g.DrawRectangle(Pens.White, Global.button1Anfang.X, Global.button6Anfang.Y, 200, 40);
			g.DrawString("Wallverbesserung", SystemFonts.DefaultFont, Brushes.White, Global.button1Anfang.X, Global.button6Anfang.Y);

			g.DrawRectangle(Pens.White, Global.button2Anfang.X, Global.button6Anfang.Y, 200, 40);
			g.DrawString("Turmverbesserung", SystemFonts.DefaultFont, Brushes.White, Global.button2Anfang.X, Global.button6Anfang.Y);

			g.DrawRectangle(Pens.White, Global.button6Anfang.X, Global.button6Anfang.Y, 200, 40);
			g.DrawString("Spezialverbesserung", SystemFonts.DefaultFont, Brushes.White, Global.button6Anfang);


			//Rote auswahlmarkierung zeichnen, + die ausrichtung des Dynamischen Turms
			if (gebaudeauswahl != null) gebaudeauswahl.zeichnenAusegwählt(g);
			else if (auswahl.X != 1000)
			{
				g.DrawRectangle(Pens.Red, auswahl.X * Global.blockgroesse - 2, auswahl.Y * Global.blockgroesse - 1, Global.blockgroesse + 2, Global.blockgroesse + 2);

				g.DrawImage(vorschau1.bild, new Point(Global.button1Anfang.X + 165, Global.button1Anfang.Y + 5));
				if (vorschau1.turmD != null)
				{
					g.DrawImage(vorschau1.bildD, new Point(Global.button1Anfang.X + 168, Global.button1Anfang.Y + 9));
				}
				g.DrawImage(vorschau2.bild, new Point(Global.button2Anfang.X + 165, Global.button1Anfang.Y + 5));
				if (vorschau2.turmD != null)
				{
					g.DrawImage(vorschau2.bildD, new Point(Global.button2Anfang.X + 168, Global.button1Anfang.Y + 9));
				}
				g.DrawImage(vorschau3.bild, new Point(Global.button6Anfang.X + 165, Global.button1Anfang.Y + 5));
				if (vorschau3.turmD != null)
				{
					g.DrawImage(vorschau3.bildD, new Point(Global.button6Anfang.X + 168, Global.button1Anfang.Y + 9));
				}
			}

			//Projektile mit Rotation zeichnen
			for (int i = 0; i < karteDynamisch.karteKD.Count; i++)
			{
				karteDynamisch.karteKD[i].zeichnen(g);
			}

			//dragDrop linie falls gerade ein DragDrop event ist.
			if (dragDrop && gebaudeauswahl.turmD != null)
			{
				g.DrawLine(new Pen(Brushes.Red, 1), gebaudeauswahl.koordinatenPixel.X + Global.blockgroesse / 2, gebaudeauswahl.koordinatenPixel.Y + Global.blockgroesse / 2, zeichenfenster.PointToClient(Cursor.Position).X, zeichenfenster.PointToClient(Cursor.Position).Y);
			}
			if (bezierDragDrop && gebaudeauswahl != null && gebaudeauswahl.turmD != null)
			{
				if (bezierPunkt3.X != 10000)
				{
					g.DrawLine(new Pen(Brushes.WhiteSmoke, 1), bezierPunkt1, bezierPunkt2);
					g.DrawLine(new Pen(Brushes.WhiteSmoke, 1), bezierPunkt3, new Point(zeichenfenster.PointToClient(Cursor.Position).X, zeichenfenster.PointToClient(Cursor.Position).Y));
					g.DrawBezier(new Pen(Brushes.Red, 1), bezierPunkt1, bezierPunkt2, bezierPunkt3, new Point(zeichenfenster.PointToClient(Cursor.Position).X, zeichenfenster.PointToClient(Cursor.Position).Y));
				}
			}
		}

		/// <summary>
		/// Geht alle dynamischen objekte durch und lässt sie einen Berechnungsschreitt weiter berechnen
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
						karteStatisch.karteS[xKugel, yKugel].schadenBekommen(kugel.schadenmachen(), kugel.projektil);//kugel macht momentan schaden aber töten nicht, client halt
						karteDynamisch.karteKD.RemoveAt(i);
					}
				}
				else karteDynamisch.karteKD.RemoveAt(i);
			}

			for (int i = 0; i < karteDynamisch.karteTD.Count; i++)
			{
				if (karteDynamisch.karteTD[i].angreifen()) karteDynamisch.karteKD.Add(karteDynamisch.karteTD[i].feuern());
			}
		}

		/// <summary>
		/// Wird aufgerufen wenn in das zeichenfenster geklickt wird (wenn die Taste runtergedrückt wird.). Stellt Buttondrücke, Blockauswahl und drag and drop fest.
		/// </summary>
		/// <param name="sender">eine Referenz zum zeichenfesnter</param>
		/// <param name="e"> Mouse event e speichert ort des events und ez.  </param>
		private void zeichenfenster_MouseDown(object sender, MouseEventArgs e)
		{

			dragDrop = false;
			if (e.Button == MouseButtons.Right)//clientinterner befehl der die auswahl resetet
			{
				gebaudeauswahl = null;
				auswahl.X = 1000;
				dragDrop = false;
				bezierDragDrop = false;

				bezierPunkt3.X = 10000;
				return;
			}
			else if (bezierDragDrop && gebaudeauswahl != null)
			{
				bezierPunkt3 = new Point(e.X, e.Y);
				return;
			}
			else
			{
				if (e.X < 600 && e.Y < 600)
				{

					if (karteStatisch.karteS[e.X / Global.blockgroesse, e.Y / Global.blockgroesse] != null)
					{
						//überprüft ob gerade ein DragDrop event gestartet wurde. Bedingungen: Gebaude mit Dynamischen Turm ausgewählt und es wurde auf die position der Gebaudeauswahl geklickt.
						if (gebaudeauswahl != null && gebaudeauswahl.turmD != null && e.X / Global.blockgroesse == gebaudeauswahl.koordinaten.X && e.Y / Global.blockgroesse == gebaudeauswahl.koordinaten.Y)
						{
							dragDrop = true;
						}
						else
						{
							gebaudeauswahl = karteStatisch.karteS[e.X / Global.blockgroesse, e.Y / Global.blockgroesse];
							auswahl.X = 1000;
						}

					}
					else
					{
						gebaudeauswahl = null;
						auswahl.X = e.X / Global.blockgroesse;
						auswahl.Y = e.Y / Global.blockgroesse;
					}
				}
				//überprüfung der buttons und aufrufen der methode die den buttonaufrug zum server sendet
				else if (e.X >= Global.button1Anfang.X && e.X < Global.button1Ende.X && e.Y >= Global.button1Anfang.Y && e.Y < Global.button1Ende.Y) tasten(Tasten.button1);//VerbesserungWall
				else if (e.X >= Global.button2Anfang.X && e.X < Global.button2Ende.X && e.Y >= Global.button2Anfang.Y && e.Y < Global.button2Ende.Y) tasten(Tasten.button2);//VerbesserungTurm
				else if (e.X >= Global.button6Anfang.X && e.X < Global.button6Ende.X && e.Y >= Global.button1Anfang.Y && e.Y < Global.button1Ende.Y) tasten(Tasten.button3);//VerbesserungSpezial
				else if (e.X >= Global.button1Anfang.X && e.X < Global.button1Ende.X && e.Y >= Global.button6Anfang.Y && e.Y < Global.button6Ende.Y) tasten(Tasten.button4);//VerbesserungStatsWall
				else if (e.X >= Global.button2Anfang.X && e.X < Global.button2Ende.X && e.Y >= Global.button6Anfang.Y && e.Y < Global.button6Ende.Y) tasten(Tasten.button5);//VerbesserungStatsTurm
				else if (e.X >= Global.button6Anfang.X && e.X < Global.button6Ende.X && e.Y >= Global.button6Anfang.Y && e.Y < Global.button6Ende.Y) tasten(Tasten.button6);//VerbesserungStatsSpezial
			}

		}

		/// <summary>
		/// Überprüfung ob ein DragDrop event lauft, und fals ja wird der einheitsvektor der schußrichtung berechnet und zu dem Server geschickt. Bei Bezierkurven werden die 4 Punkte übertragen
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void zeichenfenster_MouseUp_1(object sender, MouseEventArgs e)
		{
			if (dragDrop && gebaudeauswahl.turmD != null)
			{
				if (gebaudeauswahl.turmD.bezier)
				{
					if (e.X > 0 && e.Y > 0 && e.X <= 1200 && e.Y <= 600 && !(e.X / 30 == gebaudeauswahl.koordinaten.X && e.Y / 30 == gebaudeauswahl.koordinaten.Y))
					{
						dragDrop = false;
						bezierPunkt1 = new Point(gebaudeauswahl.koordinatenPixel.X + Global.blockgroesse / 2, gebaudeauswahl.koordinatenPixel.Y + Global.blockgroesse / 2);
						bezierPunkt2 = new Point(e.X, e.Y);
						bezierDragDrop = true;
					}
					else
					{
						bezierDragDrop = false; ;
						dragDrop = false;
					}
				}
				else
				{
					dragDrop = false;
					if (!(e.X / 30 == gebaudeauswahl.koordinaten.X && e.Y / 30 == gebaudeauswahl.koordinaten.Y))
					{
						Point vektor = new Point(e.X - gebaudeauswahl.koordinatenPixel.X - Global.blockgroesse / 2, e.Y - gebaudeauswahl.koordinatenPixel.Y - Global.blockgroesse / 2);
						float einheitsvektorX = (float)((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.X);
						float einheitsvektorY = (float)((1 / Math.Sqrt(vektor.X * vektor.X + vektor.Y * vektor.Y)) * vektor.Y);

						StringBuilder strb = new StringBuilder();
						strb.Append((char)(Befehlstyp.turmDrehen));
						strb.Append((char)(gebaudeauswahl.koordinaten.X));
						strb.Append((char)(gebaudeauswahl.koordinaten.Y));
						byte[] bufferX = BitConverter.GetBytes(einheitsvektorX);
						for (int i = 0; i < 4; i++) strb.Append((char)bufferX[i]);
						byte[] bufferY = BitConverter.GetBytes(einheitsvektorY);
						for (int i = 0; i < 4; i++) strb.Append((char)bufferY[i]);
						strb.Append((char)(spieler));
						client.Senden(strb.ToString());
					}
				}

			}
			else if (bezierDragDrop && gebaudeauswahl != null)
			{
				if (e.X > 0 && e.Y > 0 && e.X <= 1200 && e.Y <= 600 && !(e.X / 30 == gebaudeauswahl.koordinaten.X && e.Y / 30 == gebaudeauswahl.koordinaten.Y))
				{
					StringBuilder strb = new StringBuilder();
					strb.Append((char)(Befehlstyp.bezierAusrichten));
					strb.Append((char)(spieler));
					strb.Append((char)(bezierPunkt1.X));
					strb.Append((char)(bezierPunkt1.Y));
					strb.Append((char)(bezierPunkt2.X));
					strb.Append((char)(bezierPunkt2.Y));
					strb.Append((char)(bezierPunkt3.X));
					strb.Append((char)(bezierPunkt3.Y));
					strb.Append((char)(e.X));
					strb.Append((char)(e.Y));

					client.Senden(strb.ToString());

					bezierDragDrop = false;
					bezierPunkt3.X = 10000;
				}
				else
				{
					bezierDragDrop = false;
					dragDrop = false;
				}
			}
		}

		/// <summary>
		/// Überprüft den tastendruck und ruft die dementsprechende Methode auf
		/// </summary>
		private void zeichenfenster_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.KeyData)
			{
				case Keys.Q:
					tasten(Tasten.button1);//VerbesserungWall
					break;
				case Keys.W:
					tasten(Tasten.button2);//VerbesserungTurm
					break;
				case Keys.E:
					tasten(Tasten.button3);//VerbesserungSpezial
					break;
				case Keys.A:
					tasten(Tasten.button4);//VerbesserungStatsWall
					break;
				case Keys.S:
					tasten(Tasten.button5);//VerbesserungStatsTurm
					break;
				case Keys.D:
					tasten(Tasten.button6);//VerbesserungStatsSpezial
					break;
				case Keys.Escape://Clientinterner Befehl der die gebadueauswahl zurücksetzt
					reset();
					break;
				default:
					break;
			}
		}

		/// <summary>
		/// Überprüft ob der Spieler genug Geld hat und senden den richtigen String für die Tastendrücke an den server
		/// </summary>
		/// <param name="taste">Gibt den betätigten button an</param>
		public void tasten(Tasten taste)
		{
			if (client.me.geld > 0 && spielLauft)
			{
				StringBuilder strb = new StringBuilder();

				if (auswahl.X != 1000 || gebaudeauswahl != null)
				{
					switch (taste)
					{
						case Tasten.button1:
							strb.Append((char)(Befehlstyp.verbessern));
							if (auswahl.X != 1000)
							{
								strb.Append((char)(auswahl.X));
								strb.Append((char)(auswahl.Y));
							}
							else
							{
								strb.Append((char)(gebaudeauswahl.koordinaten.X));
								strb.Append((char)(gebaudeauswahl.koordinaten.Y));
							}
							strb.Append((char)(Typ.wall));
							strb.Append((char)(spieler));
							client.Senden(strb.ToString());
							break;
						case Tasten.button2:
							strb.Append((char)(Befehlstyp.verbessern));
							if (auswahl.X != 1000)
							{
								strb.Append((char)(auswahl.X));
								strb.Append((char)(auswahl.Y));
							}
							else
							{
								strb.Append((char)(gebaudeauswahl.koordinaten.X));
								strb.Append((char)(gebaudeauswahl.koordinaten.Y));
							}
							strb.Append((char)(Typ.turm));
							strb.Append((char)(spieler));
							client.Senden(strb.ToString());
							break;
						case Tasten.button3:
							strb.Append((char)(Befehlstyp.verbessern));
							if (auswahl.X != 1000)
							{
								strb.Append((char)(auswahl.X));
								strb.Append((char)(auswahl.Y));
							}
							else
							{
								strb.Append((char)(gebaudeauswahl.koordinaten.X));
								strb.Append((char)(gebaudeauswahl.koordinaten.Y));
							}
							strb.Append((char)(Typ.spezial));
							strb.Append((char)(spieler));
							client.Senden(strb.ToString());
							break;
						case Tasten.button4:
							strb.Append((char)(Befehlstyp.verbessernStats));
							if (auswahl.X != 1000)
							{
							}
							else
							{
								strb.Append((char)(gebaudeauswahl.koordinaten.X));
								strb.Append((char)(gebaudeauswahl.koordinaten.Y));
								strb.Append((char)(Typ.wall));
								strb.Append((char)(spieler));
								client.Senden(strb.ToString());
							}
							break;
						case Tasten.button5:
							strb.Append((char)(Befehlstyp.verbessernStats));
							if (auswahl.X != 1000)
							{
							}
							else
							{
								strb.Append((char)(gebaudeauswahl.koordinaten.X));
								strb.Append((char)(gebaudeauswahl.koordinaten.Y));
								strb.Append((char)(Typ.turm));
								strb.Append((char)(spieler));
								client.Senden(strb.ToString());
							}
							break;
						case Tasten.button6:
							strb.Append((char)(Befehlstyp.verbessernStats));
							if (auswahl.X != 1000)
							{
							}
							else
							{
								strb.Append((char)(gebaudeauswahl.koordinaten.X));
								strb.Append((char)(gebaudeauswahl.koordinaten.Y));
								strb.Append((char)(Typ.spezial));
								strb.Append((char)(spieler));
								client.Senden(strb.ToString());
							}
							break;
						default:
							break;
					}
				}
			}
		}
		/// <summary>
		/// Verbessert einen Turm
		/// </summary>
		/// <param name="typ">Der Verbesserungstyp</param>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spielerauswahl">Der Spieler dem der Block gehört/Gehören soll</param>
		public void verbesserung(Typ typ, int x, int y, Spieler spielerauswahl)
		{
			if (spielerauswahl == spieler)
			{
				switch (typ)
				{
					case Typ.wall: verbesserungWall(x, y, spielerauswahl);
						break;
					case Typ.turm: verbesserungTurm(x, y, spielerauswahl);
						break;
					case Typ.spezial: verbesserungSpezial(x, y, spielerauswahl);
						break;
				}
			}
			else
			{
				switch (typ)
				{
					case Typ.wall: verbesserungWall(Global.feldgroesse * 2 - x - 1, y, spielerauswahl);
						break;
					case Typ.turm: verbesserungTurm(Global.feldgroesse * 2 - x - 1, y, spielerauswahl);
						break;
					case Typ.spezial: verbesserungSpezial(Global.feldgroesse * 2 - x - 1, y, spielerauswahl);
						break;
				}
			}
		}

		/// <summary>
		/// Verbessert einen Block mit dem Wallupgrade oder erschafft einen Wall
		/// </summary>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spieleraktuell">Der Spieler dem der Block gehört/Gehören soll</param>
		public void verbesserungWall(int x, int y, Spieler spieleraktuell)
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
						if (spieleraktuell == spieler)
						{
							gebaudeauswahl = block;
							auswahl.X = 1000;
						}
					}
					else
					{
						karteStatisch.karteS[x, y] = block;//überschreiben des alten Turms mit dem neuen
						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
						if (spieleraktuell == spieler)
						{
							gebaudeauswahl = block;
							auswahl.X = 1000;
						}
					}
				}
			}
			else
			{
				karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Wall(new Point(x, y), spieleraktuell, 0, 0, 0);//erzeugen von objekt

				Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
				if (turmD != null) karteDynamisch.karteTD.Add(turmD);
				if (spieleraktuell == spieler)
				{
					gebaudeauswahl = karteStatisch.karteS[x, y];
					auswahl.X = 1000;
				}
			}
			hintergrundAktuell = false;
		}

		/// <summary>
		/// Verbessert einen Block mit dem Turmupgrade oder erschafft einen Wall
		/// </summary>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spieleraktuell">Der Spieler dem der Block gehört/Gehören soll</param>
		public void verbesserungTurm(int x, int y, Spieler spieleraktuell)
		{
			if (karteStatisch.karteS[x, y] != null)
			{
				Statisch.Block block = karteStatisch.karteS[x, y].verbessern(Typ.turm);
				if (block != null)
				{
					if (karteStatisch.karteS[x, y].turmD != null)
					{
						karteDynamisch.karteTD.Remove(karteStatisch.karteS[x, y].turmD);//löschen des dynamischen objekts fals vorhanden
						karteStatisch.karteS[x, y] = block;//überschreiben von dem alten turm mit dem neuen

						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
						if (spieleraktuell == spieler)
						{
							gebaudeauswahl = block;
							auswahl.X = 1000;
						}
					}
					else
					{
						karteStatisch.karteS[x, y] = block;//überschreiben des alten Turms mit dem neuen
						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
						if (spieleraktuell == spieler)
						{
							gebaudeauswahl = block;
							auswahl.X = 1000;
						}
					}
				}
			}
			else
			{
				karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Turm(new Point(x, y), spieleraktuell, 0, 0, 0);//erzeugen von objekt

				Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
				if (turmD != null) karteDynamisch.karteTD.Add(turmD);
				if (spieleraktuell == spieler)
				{
					gebaudeauswahl = karteStatisch.karteS[x, y];
					auswahl.X = 1000;
				}
			}
			hintergrundAktuell = false;
		}

		/// <summary>
		/// Verbessert einen Block mit dem Spezialupgrade oder erschafft einen Wall
		/// </summary>
		/// <param name="x">Die X Koordinate an</param>
		/// <param name="y">Die Y Koordinate an</param>
		/// <param name="spieleraktuell">Der Spieler dem der Block gehört/Gehören soll</param>
		public void verbesserungSpezial(int x, int y, Spieler spieleraktuell)
		{
			if (karteStatisch.karteS[x, y] != null)
			{
				Statisch.Block block = karteStatisch.karteS[x, y].verbessern(Typ.spezial);
				if (block != null)
				{
					if (karteStatisch.karteS[x, y].turmD != null)
					{
						karteDynamisch.karteTD.Remove(karteStatisch.karteS[x, y].turmD);//löschen des dynamischen objekts fals vorhanden
						karteStatisch.karteS[x, y] = block;//überschreiben von dem alten turm mit dem neuen

						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
						if (spieleraktuell == spieler)
						{
							gebaudeauswahl = block;
							auswahl.X = 1000;
						}
					}
					else
					{
						karteStatisch.karteS[x, y] = block;//überschreiben des alten Turms mit dem neuen
						Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();
						if (turmD != null) karteDynamisch.karteTD.Add(turmD);//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
						if (spieleraktuell == spieler)
						{
							gebaudeauswahl = block;
							if (spieleraktuell == spieler) auswahl.X = 1000;
						}
					}
				}
			}
			else
			{
				karteStatisch.karteS[x, y] = new Statisch.Verbesserungen.Spezial(new Point(x, y), spieleraktuell, 0, 0, 0);//erzeugen von objekt

				Dynamisch.TurmD turmD = karteStatisch.karteS[x, y].initialisieren();//initialisieren und übergabe von dem dynamischen objekt fals eins vorhanden ist
				if (turmD != null) karteDynamisch.karteTD.Add(turmD);
				if (spieleraktuell == spieler)
				{
					gebaudeauswahl = karteStatisch.karteS[x, y];
					auswahl.X = 1000;
				}
			}
			hintergrundAktuell = false;
		}

		/// <summary>
		/// Gibt einem Block eine Statverbesserung
		/// </summary>
		/// <param name="typ">Verbesserungs Typ</param>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		/// <param name="spielerauswahl">Der Spieler der die Verbesserung durchführt.</param>
		public void verbesserungStats(Typ typ, int x, int y, Spieler spielerauswahl) //das und alles darunter ist ?? richtig, ?spieler? ?? brauch ich die methiden überhauptnoch?
		{
			if (spielerauswahl == spieler)
			{
				switch (typ)
				{
					case Typ.wall: verbesserungStatsWall(x, y);
						break;
					case Typ.turm: verbesserungStatsTurm(x, y);
						break;
					case Typ.spezial: verbesserungStatsSpezial(x, y);
						break;
				}
			}
			else
			{
				switch (typ)
				{
					case Typ.wall: verbesserungStatsWall(Global.feldgroesse * 2 - x - 1, y);
						break;
					case Typ.turm: verbesserungStatsTurm(Global.feldgroesse * 2 - x - 1, y);
						break;
					case Typ.spezial: verbesserungStatsSpezial(Global.feldgroesse * 2 - x - 1, y);
						break;
				}
			}
		}

		/// <summary>
		/// Verbessert den Deffensiven Stat eines Blockes
		/// </summary>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		public void verbesserungStatsWall(int x, int y)
		{
			if (karteStatisch.karteS[x, y] != null) karteStatisch.karteS[x, y].verbessernstats(Typ.wall);
		}

		/// <summary>
		/// Verbessert den Offensive Stat eines Blockes
		/// </summary>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		public void verbesserungStatsTurm(int x, int y)
		{
			if (karteStatisch.karteS[x, y] != null) karteStatisch.karteS[x, y].verbessernstats(Typ.turm);
		}

		/// <summary>
		/// Verbessert den Speziellen Stat eines Blockes
		/// </summary>
		/// <param name="x">Die X Koordinate</param>
		/// <param name="y">Die Y Koordinate</param>
		public void verbesserungStatsSpezial(int x, int y)
		{
			if (karteStatisch.karteS[x, y] != null) karteStatisch.karteS[x, y].verbessernstats(Typ.spezial);
		}

		/// <summary>
		/// Wird aufgerufen Wenn im Verbindungsfenster die Listbox neu geladen werden soll
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonNeuLaden_Click(object sender, EventArgs e)
		{
			client.neuLaden();
		}
		/// <summary>
		/// Wird aufgerufen wenn im Verbindungsfenster mit dem in der Listbox ausgewählten server verbunden werden soll
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonVerbinden_Click(object sender, EventArgs e)
		{
			client.Verbinden();
		}

		/// <summary>
		/// Wird bei dem schließen des Cleints aufgerufen, und sorgt dafür das die Threads beendet werden
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			running = false;
			client.tcpclient.Close();
		}

		/// <summary>
		/// Resetet alles damit mann ein neues spiel starten kann
		/// </summary>
		public void reset()
		{
			client.tcpclient.Close();

			baumodus = false;
			zielmodus = false;
			spielLauft = false;
			dragDrop = false;
			bezierDragDrop = false;
			verbindungGetrennt = false;
			auswahl.X = 1000;
			bezierPunkt3.X = 10000;

			client = new Kommunikation.Client(this);
			client.me.geld = Global.startgeld;
			hintergrundAktuell = false;
			panel1.Visible = true;
			panel1.Focus();
			listBox1.Items.Clear();
		}

		/// <summary>
		/// Ermöglicht es auf eine Ip zu verbinden
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void buttonIpVerbinden_Click(object sender, EventArgs e)
		{
			try
			{
				int port2 = int.Parse(port.Text);
				client.Verbinden(ip.Text, port2);
			}
			catch (Exception)
			{
			}
		}
	}
}
