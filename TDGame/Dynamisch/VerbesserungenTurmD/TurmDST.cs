using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Dynamisch.VerbesserungenTurmD
{
    /// <summary>
    /// Die Dynamische Turmklasse für den Spezialturm mit der Angriffsgeschwidigkeiterweiterung,welcher die Angrifsgeschwidigkeit von getroffenen Türmen erhöht..
    /// </summary>
    class TurmDST : TurmD
    {

        public TurmDST(Point neueKoordinaten, Spieler neuSpieler)
            : base(neueKoordinaten, neuSpieler)
        {
        }
        /// <summary>
        /// Feuert Angrifsgeschwindigkeiterhöhende Projektile ab.
        /// </summary>
        /// <returns></returns>
        public override Kugel feuern()
        {
            return new Kugel(schaden, vektorX, vektorY, speed, koordinatenMitte.X, koordinatenMitte.Y, spieler, Projektileffekt.angriffsgeschwindigkeit, kugelBild);
        }
    }
}
