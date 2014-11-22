using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Dynamisch.VerbesserungenTurmD
{
	/// <summary>
	/// Die Dynamische Turmklasse für den Spezialturm der andere Türme mit energie versorgt.
	/// </summary>
	class TurmDS : TurmD
	{
		public TurmDS(Point neueKoordinaten, Spieler neuSpieler)
			: base(neueKoordinaten, neuSpieler)
		{
		}

		/// <summary>
		/// Feuert EnergieProjektile ab die die Energie der Getroffenen Gebäude erhöhen.
		/// </summary>
		/// <returns></returns>
		public override Kugel feuern()
		{
			int zwischenspeicher = energie;
			energie = (int)(energie * 0.8);  //sorgt für einen einigermaßen gleichmäßigen energieoutput
			return new Kugel(zwischenspeicher - energie, vektorX, vektorY, speed, koordinatenMitte.X, koordinatenMitte.Y, spieler, Projektileffekt.energie, kugelBild);
		}
	}
}
