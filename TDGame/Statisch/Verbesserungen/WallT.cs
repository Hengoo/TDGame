using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Das ist die offensive Verbesserung des Walles und hat die Normale Gras Textur, und ist dadurch nicht von außen zu erkennen (für beide Spieler )
	/// </summary>
	class WallT : TDGame.Statisch.Block
	{
		public WallT(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
			: base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
		{
		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 400;
			maxSchild = Global.standartSchild * 2;
			schild = 100;
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.Grass10);
				if (koordinaten.X >= Global.feldgroesse)
				{
					bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
			}
			statVerbesserungInitialisieren();
			return null;
		}

		public override Block verbessern(Typ verbesserungstyp)
		{
			return null;
		}
	}
}