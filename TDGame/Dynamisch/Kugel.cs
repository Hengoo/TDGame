using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;


namespace TDGame.Dynamisch
{
    /// <summary>
    /// Klasse der sich bewegenden Kugel Objekte. Andere Mobile Klassen erben von dieser
    /// </summary>
    public class Kugel
    {
        /// <summary>
        /// Gibt den Schaden an den Die Kugel beim Aufprall verursachen wird. Bei anderen Kugeleffekten die intensität des entsprechenden effekts.
        /// </summary>
        public int schaden;
        /// <summary>
        /// Speichert den Projektiltypen und damit den effekt den die kugel verursachen wird.
        /// </summary>
        public Projektileffekt projektil;
        /// <summary>
        /// Die X und Y Koordinaten des Ursprungsgebaudes, damit positive Effekte nicht auf sich selber übertragen werden.
        /// </summary>
        public Point ursprungskoordinaten;
        /// <summary>
        /// X und Y Koordianten in pixel
        /// </summary>
        public Point koordinaten;
        /// <summary>
        /// Genaue X koordinate, wird nur zur berechnung benötigt
        /// </summary>
        public float koordinateX;
        /// <summary>
        /// Genaue Y koordinate, wird nur zur berechnung benötigt
        /// </summary>
        public float koordinateY;
        /// <summary>
        /// Die x geschwindigkeit in pixel pro berechnung(frame)
        /// </summary>
        public float xGeschwindigkeit;
        /// <summary>
        /// Die x geschwindigkeit in pixel pro berechnung(frame)
        /// </summary>
        public float yGeschwindigkeit;
        /// <summary>
        /// Speichert welchem Spieler diese Kugel gehört
        /// </summary>
        public Spieler spieler;
        /// <summary>
        /// Speichert Das Bild dieser Kugel
        /// </summary>
        private Bitmap bild;

        /// <summary>
        /// Erstellt und Initialisiert alle Attribute des Kugel Objekts
        /// </summary>
        /// <param name="neuSchaden">Gibt den Schaden an den Die Kugel beim Aufprall verursachen wird.</param>
        /// <param name="neuXGeschwindigkeit">Gibt die X Geschwindigkeit an</param>
        /// <param name="neuYGeschwindigkeit">Gibt die Y Geschwindigkeit an</param>
        /// <param name="Geschwindigkeit">Gibt den Faktor an mit dem Die Geschwidigkeits werte Multipliziert werden um die genauen Geschwidigkeitswerte zu bekommen</param>
        /// <param name="neuX">Die X Position in Pixel</param>
        /// <param name="neuY">Die Y Position in Pixel</param>
        /// <param name="neuSpieler">Der Spieler dem die Kugel gehören wird</param>
        /// <param name="neuProjektil">Der Projektiltyp der kugel</param>
        /// <param name="neuBild">Das Bild mitdem die Kugel gezeichnet wird</param>
        public Kugel(int neuSchaden, float neuXGeschwindigkeit, float neuYGeschwindigkeit, int Geschwindigkeit, int neuX, int neuY, Spieler neuSpieler, Projektileffekt neuProjektil, Bitmap neuBild)
        {
            schaden = neuSchaden;
            projektil = neuProjektil;
            xGeschwindigkeit = neuXGeschwindigkeit * Geschwindigkeit;
            yGeschwindigkeit = neuYGeschwindigkeit * Geschwindigkeit;
            koordinateX = neuX;
            koordinateY = neuY;
            ursprungskoordinaten = new Point(neuX / Global.blockgroesse, neuY / Global.blockgroesse);
            bild = new Bitmap (neuBild);
            koordinaten = new Point(neuX, neuY); //hoch runter verschieben je nach dem wie groß die kugel ist fehlt fehler :problem war schon verwendung, vieleicht bild= new ... neubild??;
            spieler = neuSpieler;
        }

        /// <summary>
        /// Bewegt die Kugel einen Brechenungsschritt weiter.
        /// Wird von Klassen überschrieben wenn die Kugeln nicht mehr gerade fliegen sollen.
        /// </summary>
        /// <returns>Gibt die neuen Koordinaten zur Kollisionsüberprüfung zurück</returns>
        public virtual Point bewegen()
        {
            koordinateX += xGeschwindigkeit;
            koordinateY += yGeschwindigkeit;
            koordinaten.X = (int)koordinateX;
            koordinaten.Y = (int)koordinateY;
            return koordinaten;
        }

        /// <summary>
        /// Gibt die Koordinaten an an welchem Punkt die Kugel nach einem Berechnungsschritt sein würde, um zu überprüfen ob es bereits außerhalb der Karte ist.
        /// Wird von Klassen überschrieben wenn die Kugeln nicht mehr gerade fliegen sollen.
        /// </summary>
        /// <returns>Gibt die Koordinaten an an welchem Punkt die Kugel nach einem Berechnungsschritt sein würde</returns>
        public virtual Point naechsteKoordinaten()
        {
            return new Point((int)(koordinateX + xGeschwindigkeit), (int)(koordinateY + yGeschwindigkeit));
        }

        /// <summary>
        /// Zeichnet das Kugelobjekt. Wird von anderen Klassen überschrieben
        /// </summary>
        /// <param name="g"></param>
        public void zeichnen(Graphics g)
        {
            {
                g.DrawImage(Global.RotateImage(bild, (float)(Math.Atan2(yGeschwindigkeit, xGeschwindigkeit) * 180 / Math.PI)), koordinaten);
            }

        }

        /// <summary>
        /// Gibt den Schaden zurück den die Kugel machen würde.
        /// </summary>
        /// <returns>Der schaden der Kugel</returns>
        public int schadenmachen()
        {
            return schaden;
        }

        /// <summary>
        /// gibt die Koordinaten im Blockraster-Werten zurück
        /// </summary>
        /// <returns>Gibt die Koordinaten im Blockraster an</returns>
        public Point getKoordinatenFeld()
        {
            return new Point(koordinaten.X / Global.blockgroesse, koordinaten.Y / Global.blockgroesse);
        }
    }
}
