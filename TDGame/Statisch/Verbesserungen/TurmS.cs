using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Spezialverbesserung des Turmes. Sehr wenig Leben, sehr hohe energie Kosten und Abklingzeiten, dafür besondere flugeigenschaften
	/// </summary>
	class TurmS : Statisch.Block
	{
		public TurmS(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
			: base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
		{
		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 42;
			maxSchild = Global.standartSchild;
			turmD = new Dynamisch.VerbesserungenTurmD.TurmDTS(koordinatenPixel, spieler);
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.Wall);
				if (koordinaten.X >= Global.feldgroesse)
				{
					bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
				bildD = new Bitmap(Properties.Resources.Bezier);
				turmD.initialisieren(10, 2, 400, 1000, bildD, new Bitmap(Properties.Resources.Angriff));
			}
			else
			{
				turmD.initialisieren(10, 2, 400, 1000, bildD, null);
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