using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
    /// <summary>
    /// Spezialverbesserung des Spezial TUrmes. Regeneriert mehr energie.
    /// </summary>
    class SpezialS : Block
    {
        public SpezialS(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
            : base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
        {
        }

        public override Dynamisch.TurmD initialisieren()
        {
            hP = 100;
            maxSchild = Global.standartSchild;
            bild = new Bitmap(Properties.Resources.Wall);
            if (koordinaten.X >= Global.feldgroesse)
            {
                bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            bildD = new Bitmap(Properties.Resources.Spezial);
            turmD = new Dynamisch.VerbesserungenTurmD.TurmDS(koordinatenPixel, spieler);
            turmD.initialisieren(0, 1, 5, -15, bildD, new Bitmap(Properties.Resources.EnergieKugel));
            statVerbesserungInitialisieren();
            return turmD;
        }

        public override Block verbessern(Typ verbesserungstyp)
        {
            return null;
        }
    }
}
