using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Das ist der Deffensive Blocktyp.
	/// </summary>
	class Wall : TDGame.Statisch.Block
	{
		public Wall(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
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
				bild = new Bitmap(Properties.Resources.Wall3d);
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
			switch (verbesserungstyp)
			{
				case Typ.wall:
					return new Verbesserungen.WallW(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
				case Typ.turm:
					return new Verbesserungen.WallT(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
				case Typ.spezial:
					return new Verbesserungen.WallS(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
				default:
					return null;
			}
		}
	}
}
