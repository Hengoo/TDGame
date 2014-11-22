using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Das ist die sezielle Verbesserung des Walles,Vereinigt hohe Hp und einen höheren Schild als die anderen Deffensiven Türme.
	/// </summary>
	class WallS : TDGame.Statisch.Block
	{
		public WallS(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
			: base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
		{
		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 400;
			maxSchild = Global.standartSchild * 3;
			schild = 400;
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.WallS);
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