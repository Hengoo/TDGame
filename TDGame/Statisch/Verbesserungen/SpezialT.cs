using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
    /// <summary>
    /// Turm Verbesserung des Spezial TUrms, Projektile erhöhen die Angriffsgeschwindigkeit.
    /// </summary>
    class SpezialT : Block
    {
        public SpezialT(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
            : base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
        {
        }

        public override Dynamisch.TurmD initialisieren()
        {
            hP = 70;
            maxSchild = Global.standartSchild;
            bild = new Bitmap(Properties.Resources.Wall);
            if (koordinaten.X >= Global.feldgroesse)
            {
                bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            bildD = new Bitmap(Properties.Resources.AngriffsSpeedTurm);
            turmD = new Dynamisch.VerbesserungenTurmD.TurmDST(koordinatenPixel, spieler);
            turmD.initialisieren(0, 1, 50, 600, bildD, new Bitmap(Properties.Resources.Angriffsspeed));
            statVerbesserungInitialisieren();
            return turmD;
        }

        public override Block verbessern(Typ verbesserungstyp)
        {
            return null;
        }
    }
}
