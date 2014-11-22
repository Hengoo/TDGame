using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
    /// <summary>
    /// Der Spezialturm welcher energieregenerative Geschoße abfeuert.
    /// </summary>
    class Spezial : Block
    {
        public Spezial(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
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
            bildD = new Bitmap(Properties.Resources.Spezial);
            turmD = new Dynamisch.VerbesserungenTurmD.TurmDS(koordinatenPixel, spieler);
            turmD.initialisieren(0, 1, 5, -5, bildD, new Bitmap(Properties.Resources.EnergieKugel));
            statVerbesserungInitialisieren();
            return turmD;
        }

        public override Block verbessern(Typ verbesserungstyp)
        {
            switch (verbesserungstyp)
            {
                case Typ.wall:
                    return new Verbesserungen.SpezialW(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
                case Typ.turm:
                    return new Verbesserungen.SpezialT(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
                case Typ.spezial:
                    return new Verbesserungen.SpezialS(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
                default:
                    return null;
            }

        }
    }
}
