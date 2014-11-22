using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
    /// <summary>
    /// Die deffensive Verbesserung des Walles, Mehr HP und der einzige turm mit verbesserter Rüstung.
    /// </summary>
    class WallW : TDGame.Statisch.Block
    {
        public WallW(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
            : base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
        {
        }

        public override Dynamisch.TurmD initialisieren()
        {
            hP = 600;
            ruestung = 0.7;
            maxSchild = Global.standartSchild * 2;
            schild = 100;
            bild = new Bitmap(Properties.Resources.Wall3d2);
            if (koordinaten.X >= Global.feldgroesse)
            {
                bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
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