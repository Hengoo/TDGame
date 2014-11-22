using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
	/// <summary>
	/// Das unzerstörtbare Hinderniss.
	/// </summary>
	class Berg : Block
	{
		/// <summary>
		/// Das Gespiegelte Bild um während des Baumodus die andere seite zu zeichnen.
		/// </summary>
		private Bitmap bildGespiegelt;

		public Berg(Point neueKoordinaten, Spieler neuSpieler)
			: base(neueKoordinaten, neuSpieler, 0, 0, 0)
		{

		}

		public override Dynamisch.TurmD initialisieren()
		{
			hP = 42;
			ruestung = 0;//nimmt keinen schaden
			if (!Program.isServer)
			{
				bild = new Bitmap(Properties.Resources.Berg);
				if (koordinaten.X >= Global.feldgroesse)
				{
					bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
					bildGespiegelt = new Bitmap(Properties.Resources.Berg);
				}
				else
				{
					bildGespiegelt = new Bitmap(Properties.Resources.Berg);
					bildGespiegelt.RotateFlip(RotateFlipType.RotateNoneFlipX);
				}
			}
			return null;
		}

		/// <summary>
		/// Der Berg kann nicht verbessert werden.
		/// </summary>
		/// <param name="verbesserungstyp"></param>
		/// <returns></returns>
		public override Block verbessern(Typ verbesserungstyp)
		{
			return null;
		}

		/// <summary>
		/// Der Berg kann nicht Verbessert werden.
		/// </summary>
		/// <param name="verbesserungstyp"></param>
		/// <returns></returns>
		public override bool verbessernstats(Typ verbesserungstyp)
		{
			return false;
		}

		/// <summary>
		/// Veränderte ZeichenMethode damit während des Baumodus die Berge auf dem Gegnerischen Gebiet gezeichnet werden.
		/// </summary>
		/// <param name="g"></param>
		public override void zeichnenBaumodus(Graphics g)
		{
			g.DrawImage(bild, koordinatenPixel);
			g.DrawImage(bildGespiegelt, (Global.feldgroesse * 2 - koordinaten.X - 1) * Global.blockgroesse, koordinatenPixel.Y);
		}
	}
}