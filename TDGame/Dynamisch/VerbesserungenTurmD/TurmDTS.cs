using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Dynamisch.VerbesserungenTurmD
{
    /// <summary>
    /// Klasse für den Bezierkurventurm, welcher BezierkurvenKugeln abfeuert.
    /// </summary>
    class TurmDTS : TurmD
    {
        public TurmDTS(Point neueKoordinaten, Spieler neuSpieler)
            : base(neueKoordinaten, neuSpieler)
        {
            bezier = true;
        }

        /// <summary>
        /// Feuert BezierkurvenKugeln ab welche Schaden verursachen.
        /// </summary>
        /// <returns></returns>
        public override Kugel feuern()
        {
            return new VerbesserungenKugel.KugelBezier(schaden, vektorX, vektorY, speed, spieler, Projektileffekt.schaden, kugelBild, P1, P2, P3, P4);
        }
    }
}
