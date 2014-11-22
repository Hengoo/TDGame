using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Offensive verbesserung des Turmes. Sher viel mehr schaden, langsamerer angriff,mehr energiekosten, jedoch weniger leben
	/// </summary>
	class TurmT : Statisch.Block
	{
		public TurmT(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
			: base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
		{
		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 50;
			maxSchild = Global.standartSchild;
			turmD = new Dynamisch.TurmD(koordinatenPixel, spieler);
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.Wall);
				if (koordinaten.X >= Global.feldgroesse)
				{
					bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
				bildD = new Bitmap(Properties.Resources.TurmT);
				turmD.initialisieren(30, 5, 70, 400, bildD, new Bitmap(Properties.Resources.GausKannone));
			}
			else
			{
				turmD.initialisieren(30, 5, 70, 400, bildD, null);
			}
			statVerbesserungInitialisieren();
			return turmD;
		}

		public override Block verbessern(Typ verbesserungstyp)
		{
			return null;
		}
	}
}