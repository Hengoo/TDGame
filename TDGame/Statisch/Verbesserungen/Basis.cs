using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TDGame.Statisch.Verbesserungen
{
    /// <summary>
    /// Die Hauptbasis des Spielers. Wenn sie zerstört wird hat man verloren.
    /// </summary>
    class Basis : TDGame.Statisch.Block
    {
        /// <summary>
        /// Das Gespiegelte Bild um während des Baumodus die andere seite zu zeichnen.
        /// </summary>
        private Bitmap bildGespiegelt;

        public Basis(Point neueKoordinaten, Spieler neuSpieler, int neueschadenverbesserung, int neuehpverbesserung, int neuespezialverbesserung)
            : base(neueKoordinaten, neuSpieler, neueschadenverbesserung, neuehpverbesserung, neuespezialverbesserung)
        {
        }

        public override Dynamisch.TurmD initialisieren()
        {
            hP = 400;
            maxSchild = Global.standartSchild * 4;
            schild = 42;
            ruestung = 0.6;

            bild = new Bitmap(Properties.Resources.Basis);
            if (koordinaten.X >= Global.feldgroesse)
            {
                bild.RotateFlip(RotateFlipType.RotateNoneFlipX);
                bildGespiegelt = new Bitmap(Properties.Resources.Basis);
            }
            else
            {
                bildGespiegelt = new Bitmap(Properties.Resources.Basis);
                bildGespiegelt.RotateFlip(RotateFlipType.RotateNoneFlipX);
            }
            return null;
        }

        /// <summary>
        /// Die Basis kann nicht verbessert werden.
        /// </summary>
        /// <param name="verbesserungstyp"></param>
        /// <returns></returns>
        public override Block verbessern(Typ verbesserungstyp)
        {
            return null;
        }
        /// <summary>
        /// DIe Basis kann nicht verbessert werden.
        /// </summary>
        /// <param name="verbesserungstyp"></param>
        /// <returns></returns>
        public override bool verbessernstats(Typ verbesserungstyp)
        {
            return false;
        }
        /// <summary>
        /// Veränderte zeichenMethode damit während des Baumodus die Basis des Gegners gezeichnet wird.
        /// </summary>
        /// <param name="g"></param>
        public override void zeichnenBaumodus(Graphics g)
        {
            g.DrawImage(bild, koordinatenPixel);
            g.DrawImage(bildGespiegelt, (Global.feldgroesse * 2 - koordinaten.X - 1) * Global.blockgroesse, koordinatenPixel.Y);
        }
    }
}
