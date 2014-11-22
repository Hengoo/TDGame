using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Wallverbesserung des Spezialturms, Projektile erhöhen die Schilde
	/// </summary>
	class SpezialW : Block
	{
		public SpezialW(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
			: base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
		{
		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 150;
			maxSchild = Global.standartSchild;
			turmD = new Dynamisch.VerbesserungenTurmD.TurmDSW(koordinatenPixel, spieler);
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.Wall);
				if (koordinaten.X >= Global.feldgroesse)
				{
					bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
				bildD = new Bitmap(Properties.Resources.SchildTurm);
				turmD.initialisieren(15, 2, 30, 250, bildD, new Bitmap(Properties.Resources.SchildKugel));
			}
			else
			{
				turmD.initialisieren(15, 2, 30, 250, bildD, null);
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
