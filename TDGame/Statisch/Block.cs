using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch
{
	/// <summary>
	/// Klasse von der jede Statische Spielfklasse erbt
	/// </summary>
	public abstract class Block
	{
		/// <summary>
		/// Die Momentanen hp (Healtpoints, Lebenspunkte) des Turm
		/// </summary>
		public int hP;
		/// <summary>
		/// der momentane Schild des Gebäudes
		/// </summary>
		public int schild;
		/// <summary>
		/// Die Maximalen Schilde des Gebäudes
		/// </summary>
		public int maxSchild;
		/// <summary>
		/// Die Position des gebaudes im Block array
		/// </summary>
		public Point koordinaten;
		/// <summary>
		/// Die postition des gebaudes in pixel angegeben
		/// </summary>
		public Point koordinatenPixel;
		/// <summary>
		/// Ein faktor der den erhaltenen schaden reduziert. Je näher der wert an null desto besser ist die rüstung.
		/// </summary>
		public double ruestung;
		/// <summary>
		/// Speichert die Offensive Stat verbesserung
		/// </summary>
		public int schadenverbesserung;
		/// <summary>
		/// Speichert die Deffensive Stat verbesserung
		/// </summary>
		public int hpverbesserung;
		/// <summary>
		/// Speichert die Spezielle Stat verbesserung -------------------------------------------
		/// </summary>
		public int spezialverbesserung;
		/// <summary>
		/// Für die Upgradevorschau des WUpgrades
		/// </summary>
		private Block vorschau1;
		/// <summary>
		/// Für die Upgradevorschau des TUpgrades
		/// </summary>
		private Block vorschau2;
		/// <summary>
		/// Für die Upgradevorschau des SUpgrades
		/// </summary>
		private Block vorschau3;
		/// <summary>
		/// Speichert welchem Spieler der Block gehört, oder ob es zu der umgebung gehört.
		/// </summary>
		public Spieler spieler;
		/// <summary>
		/// Ojektreferenz zu dem Dynamischen Turmteil, ist nur bei Gebäuden initialisiert welche einen Dynamischen turmteil haben.
		/// </summary>
		public Dynamisch.TurmD turmD;
		/// <summary>
		/// Das Bild mit dem ddas Gebäude gezeichnet wird
		/// </summary>
		public Bitmap bild;
		/// <summary>
		/// Zwischenspeichervariable für die Übergabe der Statverbesserungen beim GebäudeUpgrade
		/// </summary>
		private int statNeuWall;
		/// <summary>
		/// Zwischenspeichervariable für die Übergabe der Statverbesserungen beim GebäudeUpgrade
		/// </summary>
		private int statNeuTurm;
		/// <summary>
		/// Zwischenspeichervariable für die Übergabe der Statverbesserungen beim GebäudeUpgrade
		/// </summary>
		private int statNeuSpezial;
		/// <summary>
		/// Das Bild mit dem der Dynamische Turmteil gezeichnet wird.
		/// </summary>
		public Bitmap bildD;

		/// <summary>
		/// Erstellt einen Block und weißt die ersten Werte zu. Danach muss der Block mit der initialisieren Methode initialisiert werden.
		/// </summary>
		/// <param name="neueKoordinaten">Die Turmposition</param>
		/// <param name="neuSpieler">Der spieler dem der Turm gehört</param>
		/// <param name="neueschadenverbesserung">Die Schadensverbesserung des Turmes</param>
		/// <param name="neuehpverbesserung">Die HP Verbesserung des Turmes</param>
		/// <param name="neuespezialverbesserung">Die Spezialverbesserung des Turmes</param>
		public Block(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
		{
			statNeuWall = neuehpverbesserung;
			statNeuTurm = neueschadenverbesserung;
			statNeuSpezial = neuespezialverbesserung;


			vorschau1 = verbessern(Typ.wall);
			vorschau2 = verbessern(Typ.turm);
			vorschau3 = verbessern(Typ.spezial);

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



			koordinaten = new Point(neueKoordinaten.X, neueKoordinaten.Y);
			koordinatenPixel = new Point(neueKoordinaten.X * Global.blockgroesse, neueKoordinaten.Y * Global.blockgroesse);
			spieler = neuSpieler;
			ruestung = 1;
		}

		/// <summary>
		/// Initialisiert den Block indem er fals vorhanden den dynamischen turm zurückgibt und die ganzen werte und Bilder beschreibt. Wird bei abgeleiteten Klassen überschrieben.
		/// </summary>
		public abstract Dynamisch.TurmD initialisieren();

		/// <summary>
		/// Wird nach der initialisierungs Methode aufgerufen, um die StatVerbesserungen nach einem Upgrade beizubehalten.
		/// </summary>
		protected void statVerbesserungInitialisieren()
		{
			for (; statNeuWall > 0; statNeuWall--)
			{
				verbessernstats(Typ.wall);
			}
			for (; statNeuTurm > 0; statNeuTurm--)
			{
				verbessernstats(Typ.turm);
			}
			for (; statNeuSpezial > 0; statNeuSpezial--)
			{
				verbessernstats(Typ.spezial);
			}
		}

		/// <summary>
		/// Wird aufgerufen wenn Das Gebäude schaden bekommt
		/// </summary>
		/// <param name="schaden">der schaden den der Gebäude abbekommen soll</param>
		/// <returns>Gibt true zurueck fals der Gebäude den treffer nicht mehr ueberleben wuerde</returns> 
		/// <param name="projektileffekt">Der Projektileffekt der entscheidet was der Treffer bewirkt</param>
		public bool schadenBekommen(int schaden, Projektileffekt projektileffekt)
		{
			if (projektileffekt != Projektileffekt.schaden)
			{
				switch (projektileffekt)
				{
					case Projektileffekt.energie:
						if (turmD != null)
						{
							if (turmD.energie + schaden <= Global.startenergie)
							{
								turmD.energie += schaden;
							}
							else turmD.energie = Global.startenergie;
						}
						break;
					case Projektileffekt.schild:
						if (schild + schaden <= Global.standartSchild)
						{
							schild += schaden;
						}
						else schild = Global.standartSchild;
						break;
					case Projektileffekt.angriffsgeschwindigkeit:
						if (turmD != null)
						{
							turmD.Abklingzeit2 = 10000;//sehr hohe zahl das danach sofort geschossen wird.
						}
						break;
					default:
						break;
				}
				return false;
			}
			else
			{
				double diffsh = schild - schaden;
				if (diffsh < 0)
				{
					schild = 0;
					double diffhp = hP - ((-diffsh) * ruestung);
					if (diffhp < 0)
					{
						hP = 0;
						if (Program.isServer)
							return false;
					}
					else
						hP = (int)diffhp;
				}
				else
					schild = (int)diffsh;
				return false;
			}
		}

		/// <summary>
		/// Mit dieser Methode werden die Gebaude nach der Bild Variable gezeichnet.
		/// </summary>
		/// <param name="g"> Graphics g zum ziechnen </param>
		public void zeichnen(Graphics g)
		{
			g.DrawImage(bild, koordinatenPixel);
		}

		/// <summary>
		/// Mit dieser Methode werden die Gebaude nach der Bild Variable gezeichnet(verwendet während des Baumodus), unterschied zu der zeichen Methode: wird bei Basis und Berg überschrieben das es auch die Auf der gegnerischen Seite zeichnet.
		/// </summary>
		/// <param name="g"> Graphics g zum ziechnen </param>
		public virtual void zeichnenBaumodus(Graphics g)
		{
			g.DrawImage(bild, koordinatenPixel);
		}

		/// <summary>
		/// zeichnet eine Rote markierung um das ausgewählte gebaude
		/// </summary>
		/// <param name="g">Graphics g zum ziechnen</param>
		public void zeichnenAusegwählt(Graphics g)
		{
			g.DrawRectangle(Pens.Red, koordinatenPixel.X - 2, koordinatenPixel.Y - 1, Global.blockgroesse + 2, Global.blockgroesse + 2);

			if (turmD != null && turmD.feuerbefehl)
			{
				if (turmD.bezier)
				{
					g.DrawLine(new Pen(Brushes.LightSkyBlue, 1), turmD.P1, turmD.P2);
					g.DrawLine(new Pen(Brushes.LightSkyBlue, 1), turmD.P3, turmD.P4);
					g.DrawBezier(new Pen(Brushes.Blue, 1), turmD.P1, turmD.P2, turmD.P3, turmD.P4);
				}
				else
				{
					g.DrawLine(new Pen(Brushes.Red, 1), koordinatenPixel.X + Global.blockgroesse / 2, koordinatenPixel.Y + Global.blockgroesse / 2, koordinatenPixel.X + Global.blockgroesse / 2 + turmD.vektorX * 200, koordinatenPixel.Y + Global.blockgroesse / 2 + turmD.vektorY * 200);
				}
			}
			//zeichnet die Vorschau im Hud
			if (vorschau1 != null)
			{
				g.DrawImage(vorschau1.bild, new Point(Global.button1Anfang.X + 165, Global.button1Anfang.Y + 5));
				if (vorschau1.turmD != null)
				{
					g.DrawImage(vorschau1.bildD, new Point(Global.button1Anfang.X + 168, Global.button1Anfang.Y + 9));
				}
			}
			if (vorschau2 != null)
			{
				g.DrawImage(vorschau2.bild, new Point(Global.button2Anfang.X + 165, Global.button1Anfang.Y + 5));
				if (vorschau2.turmD != null)
				{
					g.DrawImage(vorschau2.bildD, new Point(Global.button2Anfang.X + 168, Global.button1Anfang.Y + 9));
				}
			}
			if (vorschau3 != null)
			{
				g.DrawImage(vorschau3.bild, new Point(Global.button6Anfang.X + 165, Global.button1Anfang.Y + 5));
				if (vorschau3.turmD != null)
				{
					g.DrawImage(vorschau3.bildD, new Point(Global.button6Anfang.X + 168, Global.button1Anfang.Y + 9));
				}
			}
		}


		/// <summary>
		/// Upgradesmethode, wird von abgeleiteten Klassen deffiniert.
		/// </summary>
		/// <param name="verbesserungstyp">Der Typ nach dem geupgraded werden soll </param>
		/// <returns>das neue Gebäude</returns>
		public abstract Block verbessern(Typ verbesserungstyp);

		/// <summary>
		/// Verbessert in der angegeben Kategorie den status des Blocks
		/// </summary>
		/// <param name="verbesserungstyp">Gibt an was verbessert werden soll</param>
		/// <returns>Gibt an ob die Verbesserung erfolgen konnte.</returns>
		public virtual bool verbessernstats(Typ verbesserungstyp)
		{
			switch (verbesserungstyp)
			{
				case Typ.wall:
					if (hpverbesserung < 10)
					{
						hpverbesserung++;
						hP = (int)(hP * 1.1);
						return true;
					}
					break;
				case Typ.turm:
					if (schadenverbesserung < 10)
					{
						schadenverbesserung++;
						if (turmD != null)
						{
							turmD.schaden = (int)(turmD.schaden * 1.1);
						}
						else
						{
							ruestung -= 0.05;
						}
						return true;
					}
					break;

				case Typ.spezial:
					if (spezialverbesserung < 10)
					{
						spezialverbesserung++;
						if (turmD != null)
						{
							if (turmD.energiekosten > 0)
							{
								turmD.energiekosten = (int)(turmD.energiekosten * 0.95);
							}
							else
							{
								turmD.energiekosten = (int)(turmD.energiekosten - 3);
							}

						}
						else
						{
							maxSchild = (int)(maxSchild * 1.1);
						}
						return true;
					}
					break;
			}
			return false;
		}
	}
}
