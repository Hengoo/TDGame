using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
    /// <summary>
    /// Der einfache Turm
    /// </summary>
    class Turm : Statisch.Block
    {
        public Turm(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
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
            bildD = new Bitmap(Properties.Resources.Turm1);
            turmD = new Dynamisch.TurmD(koordinatenPixel, spieler);
            turmD.initialisieren(10, 2, 40, 200, bildD, new Bitmap(Properties.Resources.Kugel1));
            statVerbesserungInitialisieren();
            return turmD;
        }

        public override Block verbessern(Typ verbesserungstyp)
        {
            switch (verbesserungstyp)
            {
                case Typ.wall:
                    return new Verbesserungen.TurmW(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
                case Typ.turm:
                    return new Verbesserungen.TurmT(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
                case Typ.spezial:
                    return new Verbesserungen.TurmS(koordinaten, spieler, schadenverbesserung, hpverbesserung, spezialverbesserung);
                default:
                    return null;
            }
        }
    }
}