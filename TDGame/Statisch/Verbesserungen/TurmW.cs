using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Deffensive verbesserung des Turmes. Weniger abklingzeit, weniger energiekosten und mehr leben.
	/// </summary>
	class TurmW : Statisch.Block
	{
		public TurmW(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
			: base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
		{
		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 200;
			maxSchild = Global.standartSchild;
			turmD = new Dynamisch.TurmD(koordinatenPixel, spieler);
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.Wall3d);
				if (koordinaten.X >= Global.feldgroesse)
				{
					bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
				bildD = new Bitmap(Properties.Resources.TurmW);
				turmD.initialisieren(10, 4, 20, 97, bildD, new Bitmap(Properties.Resources.Kugel1));
			}
			else
			{
				turmD.initialisieren(10, 4, 20, 97, bildD, null);
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